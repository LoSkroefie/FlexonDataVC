using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.IO.Compression;

namespace FlexonDataVC.Storage {
    public class BinarySerializer {
        private const int BUFFER_SIZE = 4096;

        public static void SerializeDataset(string filePath, List<object[]> rows, Dictionary<string, string> schema) {
            using var fileStream = new FileStream(filePath, FileMode.Create);
            using var gzipStream = new GZipStream(fileStream, CompressionLevel.Optimal);
            using var writer = new BinaryWriter(gzipStream);

            // Write schema
            var schemaJson = JsonSerializer.Serialize(schema);
            writer.Write(schemaJson);

            // Write row count
            writer.Write(rows.Count);

            // Write rows
            foreach (var row in rows) {
                SerializeRow(writer, row, schema);
            }
        }

        public static (List<object[]> rows, Dictionary<string, string> schema) DeserializeDataset(string filePath) {
            using var fileStream = new FileStream(filePath, FileMode.Open);
            using var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
            using var reader = new BinaryReader(gzipStream);

            // Read schema
            var schemaJson = reader.ReadString();
            var schema = JsonSerializer.Deserialize<Dictionary<string, string>>(schemaJson);

            // Read row count
            int rowCount = reader.ReadInt32();

            // Read rows
            var rows = new List<object[]>(rowCount);
            for (int i = 0; i < rowCount; i++) {
                rows.Add(DeserializeRow(reader, schema));
            }

            return (rows, schema);
        }

        private static void SerializeRow(BinaryWriter writer, object[] row, Dictionary<string, string> schema) {
            for (int i = 0; i < row.Length; i++) {
                var value = row[i];
                var type = schema.Values.ElementAt(i);

                switch (type.ToLower()) {
                    case "int":
                        writer.Write((int)value);
                        break;
                    case "long":
                        writer.Write((long)value);
                        break;
                    case "float":
                        writer.Write((float)value);
                        break;
                    case "double":
                        writer.Write((double)value);
                        break;
                    case "string":
                        writer.Write((string)value);
                        break;
                    case "datetime":
                        writer.Write(((DateTime)value).ToBinary());
                        break;
                    case "bool":
                        writer.Write((bool)value);
                        break;
                    default:
                        throw new ArgumentException($"Unsupported type: {type}");
                }
            }
        }

        private static object[] DeserializeRow(BinaryReader reader, Dictionary<string, string> schema) {
            var row = new object[schema.Count];
            
            for (int i = 0; i < schema.Count; i++) {
                var type = schema.Values.ElementAt(i);

                switch (type.ToLower()) {
                    case "int":
                        row[i] = reader.ReadInt32();
                        break;
                    case "long":
                        row[i] = reader.ReadInt64();
                        break;
                    case "float":
                        row[i] = reader.ReadSingle();
                        break;
                    case "double":
                        row[i] = reader.ReadDouble();
                        break;
                    case "string":
                        row[i] = reader.ReadString();
                        break;
                    case "datetime":
                        row[i] = DateTime.FromBinary(reader.ReadInt64());
                        break;
                    case "bool":
                        row[i] = reader.ReadBoolean();
                        break;
                    default:
                        throw new ArgumentException($"Unsupported type: {type}");
                }
            }

            return row;
        }

        public static byte[] CompressData(byte[] data) {
            using var memoryStream = new MemoryStream();
            using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal)) {
                gzipStream.Write(data, 0, data.Length);
            }
            return memoryStream.ToArray();
        }

        public static byte[] DecompressData(byte[] compressedData) {
            using var compressedStream = new MemoryStream(compressedData);
            using var decompressStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using var resultStream = new MemoryStream();
            
            decompressStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }
    }
}
