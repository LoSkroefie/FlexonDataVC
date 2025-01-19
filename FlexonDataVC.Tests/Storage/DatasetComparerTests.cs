using System;
using System.Collections.Generic;
using Xunit;
using FlexonDataVC.Storage;

namespace FlexonDataVC.Tests.Storage {
    public class DatasetComparerTests {
        private readonly Dictionary<string, string> _schema = new() {
            { "ID", "int" },
            { "Name", "string" },
            { "Value", "double" }
        };

        [Fact]
        public void CompareSets_WithAddedRows_ShouldDetectAdditions() {
            // Arrange
            var original = new List<object[]> {
                new object[] { 1, "Test1", 10.5 }
            };

            var modified = new List<object[]> {
                new object[] { 1, "Test1", 10.5 },
                new object[] { 2, "Test2", 20.7 }
            };

            // Act
            var diff = DatasetComparer.CompareSets(original, modified, _schema);

            // Assert
            Assert.Single(diff.AddedRows);
            Assert.Empty(diff.RemovedRows);
            Assert.Empty(diff.ModifiedRows);
        }

        [Fact]
        public void CompareSets_WithRemovedRows_ShouldDetectRemovals() {
            // Arrange
            var original = new List<object[]> {
                new object[] { 1, "Test1", 10.5 },
                new object[] { 2, "Test2", 20.7 }
            };

            var modified = new List<object[]> {
                new object[] { 1, "Test1", 10.5 }
            };

            // Act
            var diff = DatasetComparer.CompareSets(original, modified, _schema);

            // Assert
            Assert.Empty(diff.AddedRows);
            Assert.Single(diff.RemovedRows);
            Assert.Empty(diff.ModifiedRows);
        }

        [Fact]
        public void CompareSets_WithModifiedRows_ShouldDetectModifications() {
            // Arrange
            var original = new List<object[]> {
                new object[] { 1, "Test1", 10.5 }
            };

            var modified = new List<object[]> {
                new object[] { 1, "Test1", 15.5 }
            };

            // Act
            var diff = DatasetComparer.CompareSets(original, modified, _schema);

            // Assert
            Assert.Empty(diff.AddedRows);
            Assert.Empty(diff.RemovedRows);
            Assert.Single(diff.ModifiedRows);
        }

        [Fact]
        public void FormatDiff_ShouldProduceReadableOutput() {
            // Arrange
            var original = new List<object[]> {
                new object[] { 1, "Test1", 10.5 }
            };

            var modified = new List<object[]> {
                new object[] { 1, "Test1", 15.5 },
                new object[] { 2, "Test2", 20.7 }
            };

            // Act
            var diff = DatasetComparer.CompareSets(original, modified, _schema);
            var output = DatasetComparer.FormatDiff(diff);

            // Assert
            Assert.Contains("Added Rows:", output);
            Assert.Contains("Modified Rows:", output);
            Assert.Contains("2, Test2, 20.7", output);
        }
    }
}
