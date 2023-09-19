using System;
using System.IO;

namespace flac2mp3 {

    internal class Program {

        private static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("No path specified. Exiting");
                return;
            }

            string rootPath = args[0];
            string cmdFfmpeg = "ffmpeg";

            Recurse(new DirectoryInfo(rootPath));
        }

        private static void Recurse(DirectoryInfo dir) {
            foreach (var file in dir.EnumerateFiles().Where(f => f.Extension.ToLower() == "flac"))
                ProcessFLAC(file);

            foreach (var sub in dir.EnumerateDirectories())
                Recurse(sub);
        }

        private static void ProcessFLAC(FileInfo flac) {
            Console.WriteLine(flac.FullName);
        }
    }
}