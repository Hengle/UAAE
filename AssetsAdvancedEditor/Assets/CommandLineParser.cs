using System;
using System.Collections.Generic;
using System.IO;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Assets
{
    public static class CommandLineParser
    {
        public static void Parse(string[] args)
        {
            if (args.Length < 2 || args[0].ToLower() != "uaae")
            {
                PrintError();
                return;
            }

            switch (args[1])
            {
                case "/help":
                case "/?":
                    PrintHelp();
                    break;
                case "batchimport":
                    BatchImport(args);
                    break;
                case "batchexport":
                    BatchExport(args);
                    break;
                default:
                    PrintError();
                    break;
            }
        }

        private static string GetMainFileName(IReadOnlyList<string> args)
        {
            for (var i = 1; i < args.Count; i++)
                if (!args[i].StartsWith("-"))
                    return args[i];
            return string.Empty;
        }

        private static string GetFlags(IReadOnlyList<string> args)
        {
            var flags = string.Empty;
            for (var i = 1; i < args.Count; i++)
                if (args[i].StartsWith("-"))
                    flags += args[i];
            return flags;
        }

        public static void PrintError()
        {
            Console.WriteLine(@"Command syntax error.");
            Console.WriteLine(@"Type /help or /? to see list of supported commands.");
        }

        public static void PrintHelp()
        {
            Console.WriteLine(@"Unity Assets Advanced Editor");
            Console.Beep();
            Console.WriteLine(@"[WARNING] Command line support is under development!");
            Console.WriteLine(@"[WARNING] There is a high chance that something could go wrong.");
            Console.WriteLine(@"[WARNING] Use at your own risk.\n");
            Console.WriteLine(@"  UAAE batchexport <batch file>");
            Console.WriteLine(@"  UAAE batchimport <batch file>");
            Console.WriteLine(@"  UAAE importaemip <aemip file> <directory>");
            Console.WriteLine(@"  UAAE exportaemip <aemip file> <directory>");
            Console.WriteLine(@"Import/export arguments:");
            Console.WriteLine(@"  -keepnames writes out to the exact file name in the bundle.");
            Console.WriteLine(@"      Normally, file names are prepended with the bundle's name.");
            Console.WriteLine(@"      Note: these names are not compatible with batchimport.");
            Console.WriteLine(@"  -kd keep .decomp files. When UABEA opens compressed bundles,");
            Console.WriteLine(@"      they are decompressed into .decomp files. If you want to");
            Console.WriteLine(@"      decompress bundles, you can use this flag to keep them");
            Console.WriteLine(@"      without deleting them.");
            Console.WriteLine(@"  -fd overwrite old .decomp files.");
            Console.WriteLine(@"  -md decompress into memory. Doesn't write .decomp files.");
            Console.WriteLine(@"      -kd and -fd won't do anything with this flag set.");
            Console.WriteLine(@"Batch file example:");
            Console.WriteLine(@"  -DIR C:/somefolder/bundles/");
            Console.WriteLine(@"  +FILE C:/somefolder/bundles/resources.assets");
        }

        private static string GetNextBackup(string affectedFilePath)
        {
            for (var i = 0; i < 10000; i++)
            {
                var bakName = $"{affectedFilePath}.bak{i.ToString().PadLeft(4, '0')}";
                if (!File.Exists(bakName)) return bakName;
            }

            Console.WriteLine(@"Too many backups, exiting for your safety.");
            return null;
        }

        private static void BatchExport(string[] args)
        {
            var mainFileName = GetMainFileName(args);
            var flags = GetFlags(args);
            var files = GetAllFilesFromBatchFile(mainFileName);
            foreach (var file in files)
            {
                var decompFile = $"{file}.decomp";
                var filePath = Path.GetDirectoryName(file);

                if (flags.Contains("-md"))
                    decompFile = null;

                if (!File.Exists(file))
                {
                    Console.WriteLine($@"File {file} does not exist!");
                    return;
                }

                Console.WriteLine($@"Decompressing {file}...");
                var bun = DecompressBundle(file, decompFile);

                var entryCount = bun.bundleInf6.dirInf.Length;
                for (var i = 0; i < entryCount; i++)
                {
                    var name = bun.bundleInf6.dirInf[i].name;
                    var data = BundleHelper.LoadAssetDataFromBundle(bun, i);
                    var outName = Path.Combine(filePath ?? Environment.CurrentDirectory, flags.Contains("-keepnames") ? $"{name}.assets" : $"{Path.GetFileName(file)}_{name}.assets");
                    Console.WriteLine($@"Exporting {outName}...");
                    File.WriteAllBytes(outName, data);
                }

                bun.Close();

                if (!flags.Contains("-kd") && !flags.Contains("-md") && File.Exists(decompFile) && decompFile != null)
                    File.Delete(decompFile);

                Console.WriteLine(@"Done.");
            }
        }

        private static void BatchImport(string[] args)
        {
            var mainFileName = GetMainFileName(args);
            var flags = GetFlags(args);
            var files = GetAllFilesFromBatchFile(mainFileName);
            foreach (var file in files)
            {
                var decompFile = $"{file}.decomp";

                if (flags.Contains("-md"))
                    decompFile = null;

                if (!File.Exists(file))
                {
                    Console.WriteLine($@"File {file} does not exist!");
                    return;
                }

                Console.WriteLine($@"Decompressing {file} to {decompFile}...");
                var bun = DecompressBundle(file, decompFile);

                var reps = new List<BundleReplacer>();
                var streams = new List<Stream>();

                var entryCount = bun.bundleInf6.dirInf.Length;
                for (var i = 0; i < entryCount; i++)
                {
                    var name = bun.bundleInf6.dirInf[i].name;
                    var matchName = Path.Combine(file, $"{Path.GetFileName(file)}_{name}.assets");

                    if (File.Exists(matchName))
                    {
                        var fs = File.OpenRead(matchName);
                        var length = fs.Length;
                        reps.Add(new BundleReplacerFromStream(matchName, matchName, true, fs, 0, length));
                        streams.Add(fs);
                        Console.WriteLine($@"Importing {matchName}...");
                    }
                }

                //I guess uabe always writes to .decomp even if
                //the bundle is already decompressed, that way
                //here it can be used as a temporary file. for
                //now I'll write to memory since having a .decomp
                //file isn't guaranteed here
                byte[] data;
                using (var ms = new MemoryStream())
                using (var writer = new AssetsFileWriter(ms))
                {
                    bun.Write(writer, reps);
                    data = ms.ToArray();
                }

                Console.WriteLine($@"Writing changes to {file}...");

                //uabe doesn't seem to compress here

                foreach (var stream in streams)
                    stream.Close();

                bun.Close();
                bun.reader.Close();

                File.WriteAllBytes(file, data);

                if (!flags.Contains("-kd") && !flags.Contains("-md") && File.Exists(decompFile) && decompFile != null)
                    File.Delete(decompFile);

                Console.WriteLine(@"Done.");
            }
        }

        private static AssetBundleFile DecompressBundle(string file, string decompFile)
        {
            var bun = new AssetBundleFile();

            var fs = (Stream) File.OpenRead(file);
            var reader = new AssetsFileReader(fs);

            bun.Read(reader, true);
            if (bun.bundleHeader6.GetCompressionType() != 0)
            {
                Stream nfs = decompFile switch
                {
                    null => new MemoryStream(),
                    _ => File.Open(decompFile, FileMode.Create, FileAccess.ReadWrite)
                };
                var writer = new AssetsFileWriter(nfs);
                bun.Unpack(reader, writer);

                nfs.Position = 0;
                fs.Close();

                fs = nfs;
                reader = new AssetsFileReader(fs);

                bun = new AssetBundleFile();
                bun.Read(reader);
            }

            return bun;
        }

        private static List<string> GetAllFilesFromBatchFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var allPaths = new List<string>();
            foreach (var line in lines)
                if (line.StartsWith(" +DIR "))
                {
                    var dirName = line[6..].Trim();
                    var files = Directory.GetFiles(dirName, "*", SearchOption.AllDirectories);
                    foreach (var file in files)
                        allPaths.Add(file);
                }
                else if (line.StartsWith(" -DIR "))
                {
                    var dirName = line[6..].Trim();
                    var files = Directory.GetFiles(dirName, "*", SearchOption.AllDirectories);
                    foreach (var file in files)
                        allPaths.Remove(file);
                }
                else if (line.StartsWith(" +FILE "))
                {
                    var fileName = line[7..].Trim();
                    allPaths.Add(fileName);
                }
                else if (line.StartsWith(" -FILE "))
                {
                    var fileName = line[7..].Trim();
                    allPaths.Remove(fileName);
                }

            return allPaths;
        }
    }
}