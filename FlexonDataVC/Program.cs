using System;
using FlexonDataVC.Commands;

namespace FlexonDataVC {
    class Program {
        static void Main(string[] args) {
            if (args.Length == 0 || args.Contains("--help")) {
                PrintHelp();
                return;
            }

            string command = args[0].ToLower();
            try {
                switch (command) {
                    case "init":
                        InitCommand.Execute(args);
                        break;
                    case "add":
                        AddCommand.Execute(args);
                        break;
                    case "commit":
                        CommitCommand.Execute(args);
                        break;
                    case "diff":
                        DiffCommand.Execute(args);
                        break;
                    case "rollback":
                        RollbackCommand.Execute(args);
                        break;
                    default:
                        Console.WriteLine($"Unknown command: {command}");
                        PrintHelp();
                        break;
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void PrintHelp() {
            Console.WriteLine("Flexon-DataVC: Git-like version control for datasets.");
            Console.WriteLine("Usage: datagit <command> [options]");
            Console.WriteLine("Commands:");
            Console.WriteLine("  init               Initialize a new data repository.");
            Console.WriteLine("  add <file>         Add a dataset to the repository.");
            Console.WriteLine("  commit -m <msg>    Commit changes to the dataset.");
            Console.WriteLine("  diff <version>     Show differences between versions.");
            Console.WriteLine("  rollback <version> Rollback to a previous version.");
            Console.WriteLine("Use '--help' for more details on each command.");
        }
    }
}