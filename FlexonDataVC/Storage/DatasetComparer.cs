using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexonDataVC.Storage {
    public class DatasetComparer {
        public class DatasetDiff {
            public List<object[]> AddedRows { get; set; } = new();
            public List<object[]> RemovedRows { get; set; } = new();
            public List<(object[] Original, object[] Modified)> ModifiedRows { get; set; } = new();
            public Dictionary<string, string> Schema { get; set; } = new();
        }

        public static DatasetDiff CompareSets(List<object[]> original, List<object[]> modified, Dictionary<string, string> schema) {
            var diff = new DatasetDiff { Schema = schema };
            var originalDict = original.ToDictionary(row => GetRowKey(row, 0), row => row); // Use first column as key
            var modifiedDict = modified.ToDictionary(row => GetRowKey(row, 0), row => row);

            // Find modified and added rows
            foreach (var row in modified) {
                var key = GetRowKey(row, 0);
                if (originalDict.ContainsKey(key)) {
                    if (!RowsEqual(originalDict[key], row)) {
                        diff.ModifiedRows.Add((originalDict[key], row));
                    }
                } else {
                    diff.AddedRows.Add(row);
                }
            }

            // Find removed rows
            foreach (var row in original) {
                var key = GetRowKey(row, 0);
                if (!modifiedDict.ContainsKey(key)) {
                    diff.RemovedRows.Add(row);
                }
            }

            return diff;
        }

        private static string GetRowKey(object[] row, int keyIndex) {
            return row[keyIndex]?.ToString() ?? "null";
        }

        private static bool RowsEqual(object[] row1, object[] row2) {
            if (row1.Length != row2.Length) return false;
            
            for (int i = 0; i < row1.Length; i++) {
                if (!Equals(row1[i], row2[i])) return false;
            }
            
            return true;
        }

        public static string FormatDiff(DatasetDiff diff) {
            var sb = new System.Text.StringBuilder();
            
            sb.AppendLine("Dataset Differences:");
            sb.AppendLine("-------------------");

            if (diff.ModifiedRows.Any()) {
                sb.AppendLine("\nModified Rows:");
                foreach (var (original, modified) in diff.ModifiedRows) {
                    sb.AppendLine($"- {FormatRow(original)}");
                    sb.AppendLine($"+ {FormatRow(modified)}");
                }
            }

            if (diff.AddedRows.Any()) {
                sb.AppendLine("\nAdded Rows:");
                foreach (var row in diff.AddedRows) {
                    sb.AppendLine($"+ {FormatRow(row)}");
                }
            }

            if (diff.RemovedRows.Any()) {
                sb.AppendLine("\nRemoved Rows:");
                foreach (var row in diff.RemovedRows) {
                    sb.AppendLine($"- {FormatRow(row)}");
                }
            }

            return sb.ToString();
        }

        private static string FormatRow(object[] row) {
            return string.Join(", ", row.Select(field => field?.ToString() ?? "null"));
        }
    }
}
