using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

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
            Parallel.ForEach(dir.EnumerateFiles().Where(f => f.Extension.ToLower() == ".flac"), file => {
                ProcessFLAC(file);
            });

            Parallel.ForEach(dir.EnumerateDirectories(), sub => {
                Recurse(sub);
            });
        }

        private static void ProcessFLAC(FileInfo flac) {
            string mp3 = Path.ChangeExtension(flac.FullName, ".mp3");
            Process proc = new Process();

            proc.StartInfo.FileName = "ffmpeg";
            proc.StartInfo.Arguments = $"-i \"{flac.FullName}\" -vn -c:a libmp3lame -ab 320k -y \"{mp3}\"";

            proc.Start();
            proc.WaitForExit();

            flac.Delete();
        }
    }
}