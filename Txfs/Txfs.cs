using Txfs.Impl;

namespace Txfs
{
    /// <summary>
    /// TXFS allows transactional file system access and operations.
    /// </summary>
    public static class Txfs
    {
        /// <summary>
        /// Creates a new TXFS file system entry point.
        /// </summary>
        /// <returns>A file system.</returns>
        public static IFileSystem CreateFileSystem()
        {
            return new FileSystem();
        }
    }
}
