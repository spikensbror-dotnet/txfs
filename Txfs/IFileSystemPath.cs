namespace Txfs
{
    /// <summary>
    /// Abstracts a path against the file system.
    /// </summary>
    public interface IFileSystemPath
    {
        /// <summary>
        /// The full path managed by the file system path.
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Combines the current path with a file or directory name, resulting in a new
        /// file system path.
        /// </summary>
        /// <param name="name">A file or directory name.</param>
        /// <returns>The resulting file system path.</returns>
        IFileSystemPath Child(string name);

        /// <summary>
        /// Determines if a file or directory exists at the current path.
        /// </summary>
        /// <returns>True if file or directory exists at the current path, otherwise false.</returns>
        bool Exists();

        /// <summary>
        /// Returns the file extension of the current path.
        /// </summary>
        /// <returns>The file extension of the current path, prepended with a '.'.</returns>
        string GetExtension();

        /// <summary>
        /// Returns the file name of the current path.
        /// </summary>
        /// <returns>The file name of the current path.</returns>
        string GetFileName();

        /// <summary>
        /// Returns the file name of the current path without the file extension.
        /// </summary>
        /// <returns>The file name of the current path without the file extension.</returns>
        string GetFileNameWithoutExtension();

        /// <summary>
        /// Returns all files that reside in the current path.
        /// </summary>
        /// <exception cref="System.IO.DirectoryNotFoundException">Thrown when the current path is not a directory.</exception>
        /// <returns></returns>
        IFileSystemPath[] GetFiles();

        /// <summary>
        /// Determines if the current path points to a directory.
        /// </summary>
        /// <returns>True if the current path is a directory, otherwise false.</returns>
        bool IsDirectory();

        /// <summary>
        /// Determines if the current path points to a file.
        /// </summary>
        /// <returns>True if the current path is a file, otherwise false.</returns>
        bool IsFile();

        /// <summary>
        /// Returns the parent file system path of the current path.
        /// </summary>
        /// <returns>The parent file system path of the current path.</returns>
        IFileSystemPath Parent();

        /// <summary>
        /// Reads the file at the current path, deserializing the JSON contents of it into the
        /// specified type.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Thrown when the current path is not a file.</exception>
        /// <typeparam name="T">The type to deserialize the file contents as.</typeparam>
        /// <returns>The resulting deserialized object.</returns>
        T ReadJsonFile<T>();

        /// <summary>
        /// Reads the file at the current path as text.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Thrown when the current path is not a file.</exception>
        /// <returns>The text contents of the file.</returns>
        string ReadAllText();
    }
}
