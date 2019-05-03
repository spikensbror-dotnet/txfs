using Txfs.Impl;

namespace Txfs
{
    public static class StringAsFileSystemPathExtension
    {
        /// <summary>
        /// Converts the string to an abstracted file system path.
        /// </summary>
        /// <param name="path">The file path to abstract.</param>
        /// <returns>The resulting file system abstraction.</returns>
        public static IFileSystemPath AsFileSystemPath(this string path)
        {
            return new FileSystemPath(path.ToSafePath());
        }
    }
}
