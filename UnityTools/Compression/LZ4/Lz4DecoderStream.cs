﻿using System;
using System.IO;

namespace UnityTools.Compression.LZ4
{
    public class Lz4DecoderStream : Stream
    {
        public Lz4DecoderStream(Stream input, long inputLength = long.MaxValue)
        {
            Reset(input, inputLength);
        }

        public void Reset(Stream input, long inputLength = long.MaxValue)
        {
            this.inputLength = inputLength;
            this.input = input;

            phase = DecodePhase.ReadToken;

            decodeBufferPos = 0;

            litLen = 0;
            matLen = 0;
            matDst = 0;

            inBufPos = DecBufLen;
            inBufEnd = DecBufLen;
        }

        public override void Close()
        {
            input = null;
        }

        private long inputLength;
        private Stream input;

        //because we might not be able to match back across invocations,
        //we have to keep the last window's worth of bytes around for reuse
        //we use a circular buffer for this - every time we write into this
        //buffer, we also write the same into our output buffer

        private const int DecBufLen = 0x10000;
        private const int DecBufMask = 0xFFFF;

        private const int InBufLen = 128;

        private byte[] decodeBuffer = new byte[DecBufLen + InBufLen];
        private int decodeBufferPos, inBufPos, inBufEnd;

        //we keep track of which phase we're in so that we can jump right back
        //into the correct part of decoding

        private DecodePhase phase;

        private enum DecodePhase
        {
            ReadToken,
            ReadExLiteralLength,
            CopyLiteral,
            ReadOffset,
            ReadExMatchLength,
            CopyMatch,
        }

        //state within interruptable phases and across phase boundaries is
        //kept here - again, so that we can punt out and restart freely

        private int litLen, matLen, matDst;

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || count < 0 || buffer.Length - count < offset)
                throw new ArgumentOutOfRangeException();

            if (input == null)
                throw new InvalidOperationException();
            int nRead, nToRead = count;

            var decBuf = decodeBuffer;

            //the stringy gotos are obnoxious, but their purpose is to
            //make it *blindingly* obvious how the state machine transitions
            //back and forth as it reads - remember, we can yield out of
            //this routine in several places, and we must be able to re-enter
            //and pick up where we left off!

            switch (phase)
            {
                case DecodePhase.ReadToken:
                    goto readToken;

                case DecodePhase.ReadExLiteralLength:
                    goto readExLiteralLength;

                case DecodePhase.CopyLiteral:
                    goto copyLiteral;

                case DecodePhase.ReadOffset:
                    goto readOffset;

                case DecodePhase.ReadExMatchLength:
                    goto readExMatchLength;

                case DecodePhase.CopyMatch:
                    goto copyMatch;
            }

            readToken:
            int tok;
            if (inBufPos < inBufEnd)
            {
                tok = decBuf[inBufPos++];
            }
            else
            {
                tok = ReadByteCore();
                if (tok == -1)
                    goto finish;
            }

            litLen = tok >> 4;
            matLen = (tok & 0xF) + 4;

            switch (litLen)
            {
                case 0:
                    phase = DecodePhase.ReadOffset;
                    goto readOffset;

                case 0xF:
                    phase = DecodePhase.ReadExLiteralLength;
                    goto readExLiteralLength;

                default:
                    phase = DecodePhase.CopyLiteral;
                    goto copyLiteral;
            }

            readExLiteralLength:
            int exLitLen;
            if (inBufPos < inBufEnd)
            {
                exLitLen = decBuf[inBufPos++];
            }
            else
            {
                exLitLen = ReadByteCore();
                if (exLitLen == -1)
                    goto finish;
            }

            litLen += exLitLen;
            if (exLitLen == 255)
                goto readExLiteralLength;

            phase = DecodePhase.CopyLiteral;

            copyLiteral:
            var nReadLit = litLen < nToRead ? litLen : nToRead;
            if (nReadLit != 0)
            {
                if (inBufPos + nReadLit <= inBufEnd)
                {
                    var ofs = offset;

                    for (var c = nReadLit; c-- != 0;)
                        buffer[ofs++] = decBuf[inBufPos++];

                    nRead = nReadLit;
                }
                else
                {
                    nRead = ReadCore(buffer, offset, nReadLit);
                    if (nRead == 0)
                        goto finish;
                }

                offset += nRead;
                nToRead -= nRead;

                litLen -= nRead;

                if (litLen != 0)
                    goto copyLiteral;
            }

            if (nToRead == 0)
                goto finish;

            phase = DecodePhase.ReadOffset;

            readOffset:
            if (inBufPos + 1 < inBufEnd)
            {
                matDst = (decBuf[inBufPos + 1] << 8) | decBuf[inBufPos];
                inBufPos += 2;
            }
            else
            {
                matDst = ReadOffsetCore();
                if (matDst == -1)
                    goto finish;
            }

            if (matLen == 15 + 4)
            {
                phase = DecodePhase.ReadExMatchLength;
            }
            else
            {
                phase = DecodePhase.CopyMatch;
                goto copyMatch;
            }

            readExMatchLength:
            int exMatLen;
            if (inBufPos < inBufEnd)
            {
                exMatLen = decBuf[inBufPos++];
            }
            else
            {
                exMatLen = ReadByteCore();
                if (exMatLen == -1)
                    goto finish;
            }

            matLen += exMatLen;
            if (exMatLen == 255)
                goto readExMatchLength;

            phase = DecodePhase.CopyMatch;

            copyMatch:
            var nCpyMat = matLen < nToRead ? matLen : nToRead;
            if (nCpyMat != 0)
            {
                nRead = count - nToRead;

                var bufDst = matDst - nRead;
                if (bufDst > 0)
                {
                    //offset is fairly far back, we need to pull from the buffer

                    var bufSrc = decodeBufferPos - bufDst;
                    if (bufSrc < 0)
                        bufSrc += DecBufLen;
                    var bufCnt = bufDst < nCpyMat ? bufDst : nCpyMat;

                    for (var c = bufCnt; c-- != 0;)
                        buffer[offset++] = decBuf[bufSrc++ & DecBufMask];
                }
                else
                {
                    bufDst = 0;
                }

                var sOfs = offset - matDst;
                for (var i = bufDst; i < nCpyMat; i++)
                    buffer[offset++] = buffer[sOfs++];

                nToRead -= nCpyMat;
                matLen -= nCpyMat;
            }

            if (nToRead == 0)
                goto finish;

            phase = DecodePhase.ReadToken;
            goto readToken;

            finish:
            nRead = count - nToRead;

            var nToBuf = nRead < DecBufLen ? nRead : DecBufLen;
            var repPos = offset - nToBuf;

            if (nToBuf == DecBufLen)
            {
                Buffer.BlockCopy(buffer, repPos, decBuf, 0, DecBufLen);
                decodeBufferPos = 0;
            }
            else
            {
                var decPos = decodeBufferPos;

                while (nToBuf-- != 0)
                    decBuf[decPos++ & DecBufMask] = buffer[repPos++];

                decodeBufferPos = decPos & DecBufMask;
            }
            
            return nRead;
        }

        private int ReadByteCore()
        {
            var buf = decodeBuffer;

            if (inBufPos == inBufEnd)
            {
                var nRead = input.Read(buf, DecBufLen,
                    InBufLen < inputLength ? InBufLen : (int)inputLength);
                
                if (nRead == 0)
                    return -1;

                inputLength -= nRead;

                inBufPos = DecBufLen;
                inBufEnd = DecBufLen + nRead;
            }

            return buf[inBufPos++];
        }

        private int ReadOffsetCore()
        {
            var buf = decodeBuffer;

            if (inBufPos == inBufEnd)
            {
                var nRead = input.Read(buf, DecBufLen,
                    InBufLen < inputLength ? InBufLen : (int)inputLength);
                
                if (nRead == 0)
                    return -1;

                inputLength -= nRead;

                inBufPos = DecBufLen;
                inBufEnd = DecBufLen + nRead;
            }

            if (inBufEnd - inBufPos == 1)
            {
                buf[DecBufLen] = buf[inBufPos];

                var nRead = input.Read(buf, DecBufLen + 1,
                    InBufLen - 1 < inputLength ? InBufLen - 1 : (int)inputLength);
                
                if (nRead == 0)
                {
                    inBufPos = DecBufLen;
                    inBufEnd = DecBufLen + 1;

                    return -1;
                }

                inputLength -= nRead;

                inBufPos = DecBufLen;
                inBufEnd = DecBufLen + nRead + 1;
            }

            var ret = (buf[inBufPos + 1] << 8) | buf[inBufPos];
            inBufPos += 2;

            return ret;
        }

        private int ReadCore(byte[] buffer, int offset, int count)
        {
            var nToRead = count;

            var buf = decodeBuffer;
            var inBufLen = inBufEnd - inBufPos;

            var fromBuf = nToRead < inBufLen ? nToRead : inBufLen;
            if (fromBuf != 0)
            {
                var bufPos = inBufPos;

                for (var c = fromBuf; c-- != 0;)
                    buffer[offset++] = buf[bufPos++];

                inBufPos = bufPos;
                nToRead -= fromBuf;
            }

            if (nToRead != 0)
            {
                int nRead;

                if (nToRead >= InBufLen)
                {
                    nRead = input.Read(buffer, offset,
                        nToRead < inputLength ? nToRead : (int)inputLength);
                    nToRead -= nRead;
                }
                else
                {
                    nRead = input.Read(buf, DecBufLen,
                        InBufLen < inputLength ? InBufLen : (int)inputLength);

                    inBufPos = DecBufLen;
                    inBufEnd = DecBufLen + nRead;

                    fromBuf = nToRead < nRead ? nToRead : nRead;

                    var bufPos = inBufPos;

                    for (var c = fromBuf; c-- != 0;)
                        buffer[offset++] = buf[bufPos++];

                    inBufPos = bufPos;
                    nToRead -= fromBuf;
                }

                inputLength -= nRead;
            }

            return count - nToRead;
        }

        #region Stream internals

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override void Flush()
        {
        }

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}