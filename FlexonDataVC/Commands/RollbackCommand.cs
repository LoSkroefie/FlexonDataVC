using System;
using System.IO;

namespace FlexonDataVC.Commands {
    public class RollbackCommand {
        public static void Execute(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("Usage: datagit rollback <version>");
                return;
            }

            string version = args[1];
            string repoPath = Path.Combine(Directory.GetCurrentDirectory(), ".datagit");
            string versionFile = Path.Combine(repoPath, "versions", $"version_{version}.json");
            string stagedFile = Path.Combine(repoPath, "staged.json");

            if (!File.Exists(versionFile)) {
                Console.WriteLine($"Version {version} not found.");
                return;
            }

            File.Copy(versionFile, stagedFile, true);
            Console.WriteLine($"Rolled back to version {version}.");
        }
    }
}
