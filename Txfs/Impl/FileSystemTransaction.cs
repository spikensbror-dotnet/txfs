namespace Txfs.Impl
{
    class FileSystemTransaction : IFileSystemTransaction
    {
        private readonly IFileSystemTransactionLog log = new FileSystemTransactionLog();

        private bool ShouldCommit { get; set; }

        public void Commit()
        {
            this.ShouldCommit = true;
        }

        public void Dispose()
        {
            if (this.ShouldCommit)
            {
                foreach (var action in this.log.CommitActions)
                {
                    action(); // TODO: Error handling???
                }
            }
            else
            {
                foreach (var action in this.log.RollbackActions)
                {
                    action(); // TODO: Error handling???
                }
            }
        }

        public ITransactedFileSystemPath Transact(IFileSystemPath path)
        {
            return new TransactedFileSystemPath(this, this.log, path);
        }

        public void Rollback()
        {
            this.ShouldCommit = false;
        }
    }
}
