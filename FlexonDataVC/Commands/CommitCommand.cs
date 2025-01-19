using System;
using System.IO;

namespace FlexonDataVC.Commands {
    public class CommitCommand {
        public static void Execute(string[] args) {
            if (args.Length < 3 || args[1] != "-m") {
                Console.WriteLine("Usage: datagit commit -m <message>");
                return;
            }

            string message = args[2];
            string repoPath = Path.Combine(Directory.GetCurrentDirectory(), ".datagit");
            string stagedPath = Path.Combine(repoPath, "staged.json");

            if (!File.Exists(stagedPath)) {
                Console.WriteLine("No file staged for commit.");
                return;
            }

            string metadataPath = Path.Combine(repoPath, "metadata.json");
            string versionsPath = Path.Combine(repoPath, "versions");
            Directory.CreateDirectory(versionsPath);

            int versionNumber = Directory.GetFiles(versionsPath).Length + 1;
            string versionFile = Path.Combine(versionsPath, $"version_{versionNumber}.json");

            File.Copy(stagedPath, versionFile);
            File.AppendAllText(metadataPath, $"{{\"version\": {versionNumber}, \"message\": \"{message}\", \"timestamp\": \"{DateTime.UtcNow}\"}}\n");

            Console.WriteLine($"Committed changes: {message} (version {versionNumber})");
        }
    }
}
