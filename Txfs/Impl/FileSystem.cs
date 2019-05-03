namespace Txfs.Impl
{
    class FileSystem : IFileSystem
    {
        public IFileSystemTransaction BeginTransaction()
        {
            return new FileSystemTransaction();
        }
    }
}
