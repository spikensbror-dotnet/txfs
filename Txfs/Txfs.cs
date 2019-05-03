using Txfs.Impl;

namespace Txfs
{
    public static class Txfs
    {
        public static IFileSystem CreateFileSystem()
        {
            return new FileSystem();
        }
    }
}
