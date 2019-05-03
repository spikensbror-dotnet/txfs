using System;

namespace Txfs
{
    /// <summary>
    /// A file system transaction, allowing ACID-like operations against the file system.
    /// </summary>
    public interface IFileSystemTransaction : IDisposable
    {
        /// <summary>
        /// Transacts the specified file system path, allowing write operations against the
        /// path.
        /// </summary>
        /// <param name="path">The path to transact.</param>
        /// <returns>The resulting transacted file path.</returns>
        ITransactedFileSystemPath Transact(IFileSystemPath path);

        /// <summary>
        /// Rolls back the file system changes made via the transaction.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Commits the file system changes made via the transaction.
        /// </summary>
        void Commit();
    }
}
