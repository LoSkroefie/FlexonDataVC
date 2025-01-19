using System;
using System.IO;
using FlexonDataVC.Storage;

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

            try {
                var (versionRows, versionSchema) = BinarySerializer.DeserializeDataset(versionFile);
                var (stagedRows, stagedSchema) = BinarySerializer.DeserializeDataset(stagedFile);

                var diff = DatasetComparer.CompareSets(versionRows, stagedRows, versionSchema);
                Console.WriteLine(DatasetComparer.FormatDiff(diff));
            }
            catch (Exception ex) {
                Console.WriteLine($"Error comparing datasets: {ex.Message}");
            }
        }
    }
}
