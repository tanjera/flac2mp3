using System;
using System.Diagnostics;
using System.IO;

namespace flac2mp3 {

    internal class Program {

        private static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("No path specified. Exiting");
                return;
            }

            string rootPath = args[0];
            Recurse(new DirectoryInfo(rootPath));
        }

        private static void Recurse(DirectoryInfo dir) {
            foreach (var file in dir.EnumerateFiles().Where(f => f.Extension.ToLower() == ".flac"))
                ProcessFLAC(file);

            foreach (var sub in dir.EnumerateDirectories())
                Recurse(sub);
        }

        private static void ProcessFLAC(FileInfo flac) {
            string mp3 = Path.ChangeExtension(flac.FullName, ".mp3");
            Console.WriteLine($"{flac.FullName} --> {mp3}");

            Process proc = new Process();

            proc.StartInfo.FileName = "ffmpeg";
            proc.StartInfo.Arguments = $"-i \"{flac.FullName}\" -ab 320k -map_metadata 0 -id3v2_version 3 \"mp3\"";

            proc.Start();
            proc.WaitForExit();
        }
    }
}