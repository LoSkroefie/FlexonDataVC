using System;
using System.IO;

namespace FlexonDataVC.Commands {
    public class AddCommand {
        public static void Execute(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("Usage: datagit add <file>");
                return;
            }

            string filePath = args[1];
            if (!File.Exists(filePath)) {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            string stagedPath = Path.Combine(".datagit", "staged.json");
            File.Copy(filePath, stagedPath, true);
            Console.WriteLine($"Staged {filePath} for versioning.");
        }
    }
}