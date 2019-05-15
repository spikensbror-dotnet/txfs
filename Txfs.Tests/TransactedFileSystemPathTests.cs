using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace Txfs.Tests
{
    [TestClass]
    public class TransactedFileSystemPathTests
    {
        private IFileSystemPath path;
        private IFileSystem fileSystem;

        [TestInitialize]
        public void Initialize()
        {
            this.path = Path.Combine(Path.GetTempPath(), $"txfs_{Guid.NewGuid()}").AsFileSystemPath();
            this.fileSystem = Txfs.CreateFileSystem();
        }

        [TestMethod]
        public void ShouldBeAbleToCreateAndRollback()
        {
            using (var tx = this.fileSystem.BeginTransaction())
            {
                this.Scenario(tx);

                tx.Rollback();
            }

            Assert.IsFalse(this.path.IsDirectory());
            Assert.IsFalse(Directory.Exists(this.path.FullPath));
        }

        [TestMethod]
        public void ShouldBeAbleToCreateAndCommit()
        {
            using (var tx = this.fileSystem.BeginTransaction())
            {
                this.Scenario(tx);

                tx.Commit();
            }

            Assert.IsTrue(this.path.IsDirectory());
            Assert.IsTrue(Directory.Exists(this.path.FullPath));

            Directory.Delete(this.path.FullPath, true);
        }

        /// <summary>
        /// This method should cover all functionality of Txfs.
        /// </summary>
        /// <param name="tx"></param>
        private void Scenario(IFileSystemTransaction tx)
        {
            // Equality

            Assert.AreEqual(this.path.Child("testing.txt"), this.path.Child("testing.txt"));
            Assert.AreNotEqual(this.path.Child("testing.txt"), null);
            Assert.AreNotEqual(this.path.Child("testing.txt"), 425);

            Assert.AreEqual(this.path.FullPath.GetHashCode(), this.path.GetHashCode());

            // Create directory.

            tx.Transact(this.path).CreateDirectory();

            Assert.IsTrue(this.path.IsDirectory());
            Assert.IsTrue(Directory.Exists(this.path.FullPath));

            // Create sub-directory with non-existent parent.

            var nonExistentNestedSubDirectoryPath = this.path.Child("First").Child("Second");

            tx.Transact(nonExistentNestedSubDirectoryPath).CreateDirectory();

            Assert.IsTrue(nonExistentNestedSubDirectoryPath.IsDirectory());
            Assert.IsTrue(Directory.Exists(nonExistentNestedSubDirectoryPath.FullPath));

            // Write JSON file.

            var jsonFilePath = this.path.Child("Test.json");

            tx.Transact(jsonFilePath).WriteJsonFile(new { Hello = "World" });

            Assert.IsTrue(jsonFilePath.IsFile());
            Assert.IsTrue(File.Exists(jsonFilePath.FullPath));
            Assert.AreEqual("{\"Hello\":\"World\"}", jsonFilePath.ReadAllText());

            tx.Transact(jsonFilePath).WriteJsonFile(42);

            Assert.AreEqual(42, jsonFilePath.ReadJsonFile<int>());

            // Delete sub-directory.

            tx.Transact(nonExistentNestedSubDirectoryPath).DeleteDirectory();

            Assert.IsFalse(nonExistentNestedSubDirectoryPath.IsDirectory());
            Assert.IsFalse(Directory.Exists(nonExistentNestedSubDirectoryPath.FullPath));

            Assert.IsTrue(nonExistentNestedSubDirectoryPath.Parent().IsDirectory());
            Assert.IsTrue(Directory.Exists(nonExistentNestedSubDirectoryPath.Parent().FullPath));

            // Flag file as created.

            File.WriteAllText(jsonFilePath.FullPath, "Hello world!");

            tx.Transact(jsonFilePath).FlagFileCreated();

            // Create directory from ZIP file.

            var zipFilePath = "testing.zip".AsFileSystemPath();

            var testingPath = this.path.Child("testing");

            tx.Transact(testingPath).CreateDirectoryFromZipFile(zipFilePath);

            Assert.IsTrue(testingPath.IsDirectory());
            Assert.IsTrue(Directory.Exists(testingPath.FullPath));

            Assert.AreEqual("Hello World", testingPath.Child("testing.txt").ReadAllText());

            var files = testingPath.GetFiles();
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual("testing.txt", files[0].GetFileName());
            Assert.AreEqual("testing", files[0].GetFileNameWithoutExtension());
            Assert.AreEqual(".txt", files[0].GetExtension());

            // Delete file.

            tx.Transact(jsonFilePath).DeleteFile();

            Assert.IsFalse(jsonFilePath.IsFile());
            Assert.IsFalse(File.Exists(jsonFilePath.FullPath));

            // Attempt to read non-existent file.

            var exceptions = 0;
            try
            {
                jsonFilePath.ReadAllText();
            }
            catch (FileNotFoundException)
            {
                exceptions++;
            }

            Assert.AreEqual(1, exceptions);

            // Get files from non-existent directory.

            try
            {
                nonExistentNestedSubDirectoryPath.GetFiles();
            }
            catch (DirectoryNotFoundException)
            {
                exceptions++;
            }

            Assert.AreEqual(2, exceptions);
        }
    }
}
