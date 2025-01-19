using System;
using System.IO;

namespace FlexonDataVC.Commands {
    public class DiffCommand {
        public static void Execute(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("Usage: datagit diff <version>");
                return;
            }

            string version = args[1];
            string repoPath = Path.Combine(Directory.GetCurrentDirectory(), ".datagit");
            string versionFile = Path.Combine(repoPath, "versions", $"version_{version}.json");
            string stagedFile = Path.Combine(repoPath, "staged.json");

            if (!File.Exists(versionFile) || !File.Exists(stagedFile)) {
                Console.WriteLine("Version or staged file not found.");
                return;
            }

            string versionContent = File.ReadAllText(versionFile);
            string stagedContent = File.ReadAllText(stagedFile);

            Console.WriteLine("Diff between staged and version:");
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Version Content: {versionContent}");
            Console.WriteLine($"Staged Content: {stagedContent}");
        }
    }
}
