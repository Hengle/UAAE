﻿using System.Collections.Generic;
using System.IO;

namespace UnityTools
{
    public class BundleFileInstance
    {
        public Stream stream;
        public string path;
        public string name;
        public AssetBundleFile file;
        public List<AssetsFileInstance> loadedAssetsFiles;

        public BundleFileInstance(Stream stream, string filePath, string root, bool unpackIfPacked)
        {
            this.stream = stream;
            path = Path.GetFullPath(filePath);
            name = Path.Combine(root, Path.GetFileName(path));
            file = new AssetBundleFile();
            file.Read(new AssetsFileReader(stream), true);
            if (file.Header != null && file.Header.GetCompressionType() != 0 && unpackIfPacked)
            {
                file = BundleHelper.UnpackBundle(file);
                this.stream = file.Reader.BaseStream;
            }
            loadedAssetsFiles = new List<AssetsFileInstance>();
        }

        public BundleFileInstance(FileStream stream, string root, bool unpackIfPacked)
            : this(stream, stream.Name, root, unpackIfPacked)
        {

        }
    }
}
