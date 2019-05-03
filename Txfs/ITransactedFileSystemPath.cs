namespace Txfs
{
    /// <summary>
    /// Abstracts a file system path within a transaction, allowing write operations
    /// that can be committed or rolled back.
    /// </summary>
    public interface ITransactedFileSystemPath : IFileSystemPath
    {
        // TODO: Exceptions.

        /// <summary>
        /// Creates a directory at the current path.
        /// </summary>
        void CreateDirectory();

        /// <summary>
        /// Creates a directory at the current path, extracting the contents of the specified zip file
        /// into it.
        /// </summary>
        /// <param name="zipFilePath">The file system path of the zip file to extract into the resulting directory.</param>
        void CreateDirectoryFromZipFile(IFileSystemPath zipFilePath);

        /// <summary>
        /// Deletes the directory at the current path.
        /// </summary>
        void DeleteDirectory();

        /// <summary>
        /// Deletes the file at the current path.
        /// </summary>
        void DeleteFile();

        /// <summary>
        /// Flags the file as "created" within the transaction which means that it
        /// will be managed and deleted by the transaction if rolled back.
        /// </summary>
        void FlagFileCreated();

        /// <summary>
        /// Serializes and writes the specified contents to a file at the current path, overwriting
        /// the file if it already exists.
        /// </summary>
        /// <param name="contents">The contents to serialize and write to file.</param>
        void WriteJsonFile(object contents);
    }
}
