using System;
using System.IO;

namespace FlexonDataVC.Commands {
    public class InitCommand {
        public static void Execute(string[] args) {
            string repoPath = Directory.GetCurrentDirectory() + "/.datagit";
            if (Directory.Exists(repoPath)) {
                Console.WriteLine("Repository already initialized.");
                return;
            }

            Directory.CreateDirectory(repoPath);
            File.WriteAllText(Path.Combine(repoPath, "metadata.json"), "{}");
            Console.WriteLine("Initialized a new Flexon-DataVC repository.");
        }
    }
}