﻿/*
Copyright (c) 2015 Harm Hanemaaijer <fgenfb@yahoo.com>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

namespace UnityTools
{
    internal class BPTCTables
    {
        public static readonly byte[] P2 = new byte[64 * 16]
        {
            0,0,1,1,0,0,1,1,0,0,1,1,0,0,1,1,
            0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,
            0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,
            0,0,0,1,0,0,1,1,0,0,1,1,0,1,1,1,
            0,0,0,0,0,0,0,1,0,0,0,1,0,0,1,1,
            0,0,1,1,0,1,1,1,0,1,1,1,1,1,1,1,
            0,0,0,1,0,0,1,1,0,1,1,1,1,1,1,1,
            0,0,0,0,0,0,0,1,0,0,1,1,0,1,1,1,
            0,0,0,0,0,0,0,0,0,0,0,1,0,0,1,1,
            0,0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,
            0,0,0,0,0,0,0,1,0,1,1,1,1,1,1,1,
            0,0,0,0,0,0,0,0,0,0,0,1,0,1,1,1,
            0,0,0,1,0,1,1,1,1,1,1,1,1,1,1,1,
            0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,
            0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,
            0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,
            0,0,0,0,1,0,0,0,1,1,1,0,1,1,1,1,
            0,1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,1,0,0,0,1,1,1,0,
            0,1,1,1,0,0,1,1,0,0,0,1,0,0,0,0,
            0,0,1,1,0,0,0,1,0,0,0,0,0,0,0,0,
            0,0,0,0,1,0,0,0,1,1,0,0,1,1,1,0,
            0,0,0,0,0,0,0,0,1,0,0,0,1,1,0,0,
            0,1,1,1,0,0,1,1,0,0,1,1,0,0,0,1,
            0,0,1,1,0,0,0,1,0,0,0,1,0,0,0,0,
            0,0,0,0,1,0,0,0,1,0,0,0,1,1,0,0,
            0,1,1,0,0,1,1,0,0,1,1,0,0,1,1,0,
            0,0,1,1,0,1,1,0,0,1,1,0,1,1,0,0,
            0,0,0,1,0,1,1,1,1,1,1,0,1,0,0,0,
            0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,
            0,1,1,1,0,0,0,1,1,0,0,0,1,1,1,0,
            0,0,1,1,1,0,0,1,1,0,0,1,1,1,0,0,
            0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,
            0,0,0,0,1,1,1,1,0,0,0,0,1,1,1,1,
            0,1,0,1,1,0,1,0,0,1,0,1,1,0,1,0,
            0,0,1,1,0,0,1,1,1,1,0,0,1,1,0,0,
            0,0,1,1,1,1,0,0,0,0,1,1,1,1,0,0,
            0,1,0,1,0,1,0,1,1,0,1,0,1,0,1,0,
            0,1,1,0,1,0,0,1,0,1,1,0,1,0,0,1,
            0,1,0,1,1,0,1,0,1,0,1,0,0,1,0,1,
            0,1,1,1,0,0,1,1,1,1,0,0,1,1,1,0,
            0,0,0,1,0,0,1,1,1,1,0,0,1,0,0,0,
            0,0,1,1,0,0,1,0,0,1,0,0,1,1,0,0,
            0,0,1,1,1,0,1,1,1,1,0,1,1,1,0,0,
            0,1,1,0,1,0,0,1,1,0,0,1,0,1,1,0,
            0,0,1,1,1,1,0,0,1,1,0,0,0,0,1,1,
            0,1,1,0,0,1,1,0,1,0,0,1,1,0,0,1,
            0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,
            0,1,0,0,1,1,1,0,0,1,0,0,0,0,0,0,
            0,0,1,0,0,1,1,1,0,0,1,0,0,0,0,0,
            0,0,0,0,0,0,1,0,0,1,1,1,0,0,1,0,
            0,0,0,0,0,1,0,0,1,1,1,0,0,1,0,0,
            0,1,1,0,1,1,0,0,1,0,0,1,0,0,1,1,
            0,0,1,1,0,1,1,0,1,1,0,0,1,0,0,1,
            0,1,1,0,0,0,1,1,1,0,0,1,1,1,0,0,
            0,0,1,1,1,0,0,1,1,1,0,0,0,1,1,0,
            0,1,1,0,1,1,0,0,1,1,0,0,1,0,0,1,
            0,1,1,0,0,0,1,1,0,0,1,1,1,0,0,1,
            0,1,1,1,1,1,1,0,1,0,0,0,0,0,0,1,
            0,0,0,1,1,0,0,0,1,1,1,0,0,1,1,1,
            0,0,0,0,1,1,1,1,0,0,1,1,0,0,1,1,
            0,0,1,1,0,0,1,1,1,1,1,1,0,0,0,0,
            0,0,1,0,0,0,1,0,1,1,1,0,1,1,1,0,
            0,1,0,0,0,1,0,0,0,1,1,1,0,1,1,1
        };

        public static readonly byte[] P3 = new byte[64 * 16]
        {
            0,0,1,1,0,0,1,1,0,2,2,1,2,2,2,2,
            0,0,0,1,0,0,1,1,2,2,1,1,2,2,2,1,
            0,0,0,0,2,0,0,1,2,2,1,1,2,2,1,1,
            0,2,2,2,0,0,2,2,0,0,1,1,0,1,1,1,
            0,0,0,0,0,0,0,0,1,1,2,2,1,1,2,2,
            0,0,1,1,0,0,1,1,0,0,2,2,0,0,2,2,
            0,0,2,2,0,0,2,2,1,1,1,1,1,1,1,1,
            0,0,1,1,0,0,1,1,2,2,1,1,2,2,1,1,
            0,0,0,0,0,0,0,0,1,1,1,1,2,2,2,2,
            0,0,0,0,1,1,1,1,1,1,1,1,2,2,2,2,
            0,0,0,0,1,1,1,1,2,2,2,2,2,2,2,2,
            0,0,1,2,0,0,1,2,0,0,1,2,0,0,1,2,
            0,1,1,2,0,1,1,2,0,1,1,2,0,1,1,2,
            0,1,2,2,0,1,2,2,0,1,2,2,0,1,2,2,
            0,0,1,1,0,1,1,2,1,1,2,2,1,2,2,2,
            0,0,1,1,2,0,0,1,2,2,0,0,2,2,2,0,
            0,0,0,1,0,0,1,1,0,1,1,2,1,1,2,2,
            0,1,1,1,0,0,1,1,2,0,0,1,2,2,0,0,
            0,0,0,0,1,1,2,2,1,1,2,2,1,1,2,2,
            0,0,2,2,0,0,2,2,0,0,2,2,1,1,1,1,
            0,1,1,1,0,1,1,1,0,2,2,2,0,2,2,2,
            0,0,0,1,0,0,0,1,2,2,2,1,2,2,2,1,
            0,0,0,0,0,0,1,1,0,1,2,2,0,1,2,2,
            0,0,0,0,1,1,0,0,2,2,1,0,2,2,1,0,
            0,1,2,2,0,1,2,2,0,0,1,1,0,0,0,0,
            0,0,1,2,0,0,1,2,1,1,2,2,2,2,2,2,
            0,1,1,0,1,2,2,1,1,2,2,1,0,1,1,0,
            0,0,0,0,0,1,1,0,1,2,2,1,1,2,2,1,
            0,0,2,2,1,1,0,2,1,1,0,2,0,0,2,2,
            0,1,1,0,0,1,1,0,2,0,0,2,2,2,2,2,
            0,0,1,1,0,1,2,2,0,1,2,2,0,0,1,1,
            0,0,0,0,2,0,0,0,2,2,1,1,2,2,2,1,
            0,0,0,0,0,0,0,2,1,1,2,2,1,2,2,2,
            0,2,2,2,0,0,2,2,0,0,1,2,0,0,1,1,
            0,0,1,1,0,0,1,2,0,0,2,2,0,2,2,2,
            0,1,2,0,0,1,2,0,0,1,2,0,0,1,2,0,
            0,0,0,0,1,1,1,1,2,2,2,2,0,0,0,0,
            0,1,2,0,1,2,0,1,2,0,1,2,0,1,2,0,
            0,1,2,0,2,0,1,2,1,2,0,1,0,1,2,0,
            0,0,1,1,2,2,0,0,1,1,2,2,0,0,1,1,
            0,0,1,1,1,1,2,2,2,2,0,0,0,0,1,1,
            0,1,0,1,0,1,0,1,2,2,2,2,2,2,2,2,
            0,0,0,0,0,0,0,0,2,1,2,1,2,1,2,1,
            0,0,2,2,1,1,2,2,0,0,2,2,1,1,2,2,
            0,0,2,2,0,0,1,1,0,0,2,2,0,0,1,1,
            0,2,2,0,1,2,2,1,0,2,2,0,1,2,2,1,
            0,1,0,1,2,2,2,2,2,2,2,2,0,1,0,1,
            0,0,0,0,2,1,2,1,2,1,2,1,2,1,2,1,
            0,1,0,1,0,1,0,1,0,1,0,1,2,2,2,2,
            0,2,2,2,0,1,1,1,0,2,2,2,0,1,1,1,
            0,0,0,2,1,1,1,2,0,0,0,2,1,1,1,2,
            0,0,0,0,2,1,1,2,2,1,1,2,2,1,1,2,
            0,2,2,2,0,1,1,1,0,1,1,1,0,2,2,2,
            0,0,0,2,1,1,1,2,1,1,1,2,0,0,0,2,
            0,1,1,0,0,1,1,0,0,1,1,0,2,2,2,2,
            0,0,0,0,0,0,0,0,2,1,1,2,2,1,1,2,
            0,1,1,0,0,1,1,0,2,2,2,2,2,2,2,2,
            0,0,2,2,0,0,1,1,0,0,1,1,0,0,2,2,
            0,0,2,2,1,1,2,2,1,1,2,2,0,0,2,2,
            0,0,0,0,0,0,0,0,0,0,0,0,2,1,1,2,
            0,0,0,2,0,0,0,1,0,0,0,2,0,0,0,1,
            0,2,2,2,1,2,2,2,0,2,2,2,1,2,2,2,
            0,1,0,1,2,2,2,2,2,2,2,2,2,2,2,2,
            0,1,1,1,2,0,1,1,2,2,0,1,2,2,2,0,
        };

        public static readonly byte[] AnchorIndexSecondSubset = new byte[64]
        {
            15,15,15,15,15,15,15,15,
            15,15,15,15,15,15,15,15,
            15, 2, 8, 2, 2, 8, 8,15,
             2, 8, 2, 2, 8, 8, 2, 2,
            15,15, 6, 8, 2, 8,15,15,
             2, 8, 2, 2, 2,15,15, 6,
             6, 2, 6, 8,15,15, 2, 2,
            15,15,15,15,15, 2, 2,15
        };

        public static readonly byte[] AnchorIndexSecondSubsetOfThree = new byte[64]
        {
             3, 3,15,15, 8, 3,15,15,
             8, 8, 6, 6, 6, 5, 3, 3,
             3, 3, 8,15, 3, 3, 6,10,
             5, 8, 8, 6, 8, 5,15,15,
             8,15, 3, 5, 6,10, 8,15,
            15, 3,15, 5,15,15,15,15,
             3,15, 5, 5, 5, 8, 5,10,
             5,10, 8,13,15,12, 3, 3
        };

        public static readonly byte[] AnchorIndexThirdSubset = new byte[64]
        {
            15, 8, 8, 3,15,15, 3, 8,
            15,15,15,15,15,15,15, 8,
            15, 8,15, 3,15, 8,15, 8,
             3,15, 6,10,15,15,10, 8,
            15, 3,15,10,10, 8, 9,10,
             6,15, 8,15, 3, 6, 6, 8,
            15, 3,15,15,15,15,15,15,
            15,15,15,15, 3,15,15, 8
        };

        public static readonly ushort[] AlphaWeight2 = new ushort[4]
        {
            0, 21, 43, 64
        };

        public static readonly ushort[] AlphaWeight3 = new ushort[8]
        {
            0, 9, 18, 27, 37, 46, 55, 64
        };

        public static readonly ushort[] AlphaWeight4 = new ushort[16]
        {
            0, 4, 9, 13, 17, 21, 26, 30,
            34, 38, 43, 47, 51, 55, 60, 64
        };

        public static readonly byte[] IndexBits = new byte[8]
        {
            3, 3, 2, 2, 2, 2, 4, 2
        };

        public static readonly byte[] IndexBits2 = new byte[8]
        {
            0, 0, 0, 0, 3, 2, 0, 0
        };

        public static readonly byte[] AlphaIndexBitCount = new byte[8]
        {
            3, 3, 2, 2, 3, 2, 4, 2
        };

        public static readonly byte[] ColorIndexBitCount = new byte[8]
        {
            3, 3, 2, 2, 2, 2, 4, 2
        };

        public static readonly sbyte[] ComponentsInData0 = new sbyte[8]
        {
            2, -1, 1, 1, 3, 3, 3, 2
        };

        public static readonly byte[] ColorPrecision = new byte[8]
        {
            4, 6, 5, 7, 5, 7, 7, 5
        };

        public static readonly byte[] ColorPrecisionPlusPBit = new byte[8]
        {
            5, 7, 5, 8, 5, 7, 8, 6
        };

        public static readonly byte[] AlphaPrecision = new byte[8]
        {
            0, 0, 0, 0, 6, 8, 7, 5
        };

        public static readonly byte[] AlphaPrecisionPlusPBit = new byte[8]
        {
            0, 0, 0, 0, 6, 8, 8, 6
        };

        public static readonly bool[] ModeHasPartitionBits = new bool[8]
        {
            true, true, true, true, false, false, false, true
        };

        public static readonly bool[] ModeHasPBits = new bool[8]
        {
            true, true, false, true, false, false, true, true
        };

        public static readonly byte[] NumberOfSubsets = new byte[8]
        {
            3, 2, 3, 2, 1, 1, 1, 2
        };

        public static readonly byte[] NumberOfPartitionBits = new byte[8]
        {
            4, 6, 6, 6, 0, 0, 0, 6
        };

        public static readonly byte[] NumberOfRotationBits = new byte[8]
        {
            0, 0, 0, 0, 2, 2, 0, 0
        };
    }
}
