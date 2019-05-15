using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;

namespace Txfs.Impl
{
    class TransactedFileSystemPath : FileSystemPath, ITransactedFileSystemPath
    {
        private readonly IFileSystemTransaction transaction;
        private readonly IFileSystemTransactionLog transactionLog;

        public TransactedFileSystemPath(IFileSystemTransaction transaction
            , IFileSystemTransactionLog transactionLog
            , IFileSystemPath path
            )
            : base(path.FullPath)
        {
            this.transaction = transaction;
            this.transactionLog = transactionLog;
        }

        public void CreateDirectoryFromZipFile(IFileSystemPath zipFilePath)
        {
            this.CreateDirectory();

            ZipFile.ExtractToDirectory(zipFilePath.FullPath, this.FullPath);
        }

        public void CreateDirectory()
        {
            var parentPath = this.transaction.Transact(this.Parent());
            if (!parentPath.Exists())
            {
                parentPath.CreateDirectory();
            }

            Directory.CreateDirectory(this.FullPath);

            this.transactionLog.RollbackActions.Push(() => Directory.Delete(this.FullPath, true));
        }

        public void DeleteFile()
        {
            var fileName = $"Constellation_{Guid.NewGuid()}.{this.GetFileName()}.tmp";
            var tempPath = Path.Combine(Path.GetTempPath(), fileName);

            File.Move(this.FullPath, tempPath);

            this.transactionLog.CommitActions.Push(() => File.Delete(tempPath));
            this.transactionLog.RollbackActions.Push(() => File.Move(tempPath, this.FullPath));
        }

        public void WriteJsonFile(object contents)
        {
            var previousContents = this.Exists()
                ? this.ReadAllText()
                : null;

            File.WriteAllText(this.FullPath, JsonConvert.SerializeObject(contents));

            this.transactionLog.RollbackActions.Push(() =>
            {
                if (previousContents == null)
                {
                    File.Delete(this.FullPath);
                }
                else
                {
                    File.WriteAllText(this.FullPath, previousContents);
                }
            });
        }

        public void DeleteDirectory()
        {
            var directoryName = $"Constellation_FileSystem_{Guid.NewGuid()}";
            var tempPath = Path.Combine(Path.GetTempPath(), directoryName);

            Directory.Move(this.FullPath, tempPath);

            this.transactionLog.CommitActions.Push(() => Directory.Delete(tempPath, true));
            this.transactionLog.RollbackActions.Push(() => Directory.Move(tempPath, this.FullPath));
        }

        public void FlagFileCreated()
        {
            this.transactionLog.RollbackActions.Push(() => File.Delete(this.FullPath));
        }
    }

}
