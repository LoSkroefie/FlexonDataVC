using System;
using System.Collections.Generic;
using Xunit;
using FlexonDataVC.Storage;

namespace FlexonDataVC.Tests.Storage {
    public class BinarySerializerTests {
        [Fact]
        public void SerializeDeserialize_WithVariousTypes_ShouldPreserveData() {
            // Arrange
            var schema = new Dictionary<string, string> {
                { "ID", "int" },
                { "Name", "string" },
                { "Value", "double" },
                { "Date", "datetime" },
                { "Active", "bool" }
            };

            var rows = new List<object[]> {
                new object[] { 1, "Test1", 10.5, DateTime.Now, true },
                new object[] { 2, "Test2", 20.7, DateTime.Now.AddDays(-1), false }
            };

            string tempFile = Path.GetTempFileName();

            try {
                // Act
                BinarySerializer.SerializeDataset(tempFile, rows, schema);
                var (deserializedRows, deserializedSchema) = BinarySerializer.DeserializeDataset(tempFile);

                // Assert
                Assert.Equal(schema.Count, deserializedSchema.Count);
                Assert.Equal(rows.Count, deserializedRows.Count);

                for (int i = 0; i < rows.Count; i++) {
                    Assert.Equal(rows[i].Length, deserializedRows[i].Length);
                    for (int j = 0; j < rows[i].Length; j++) {
                        Assert.Equal(rows[i][j].ToString(), deserializedRows[i][j].ToString());
                    }
                }
            }
            finally {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void CompressDecompress_ShouldPreserveData() {
            // Arrange
            var originalData = new byte[] { 1, 2, 3, 4, 5 };

            // Act
            var compressed = BinarySerializer.CompressData(originalData);
            var decompressed = BinarySerializer.DecompressData(compressed);

            // Assert
            Assert.Equal(originalData, decompressed);
        }
    }
}
