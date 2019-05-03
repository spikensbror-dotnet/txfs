namespace Txfs
{
    /// <summary>
    /// Provides transactional access to the file system.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Begins a new transaction against the file system.
        /// </summary>
        /// <returns>The resulting file system transaction.</returns>
        IFileSystemTransaction BeginTransaction();
    }
}
