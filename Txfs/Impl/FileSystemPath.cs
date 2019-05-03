using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Txfs.Impl
{
    class FileSystemPath : IFileSystemPath
    {
        public FileSystemPath(string fullPath)
        {
            this.FullPath = fullPath;
        }

        public string FullPath { get; }

        public IFileSystemPath Child(string name)
        {
            return new FileSystemPath(Path.Combine(this.FullPath, name));
        }

        public bool Exists()
        {
            return this.IsDirectory() || this.IsFile();
        }


        public IFileSystemPath[] GetFiles()
        {
            if (!this.IsDirectory())
            {
                throw new DirectoryNotFoundException($"Cannot get files for path '{this.FullPath}' as it is not a directory.");
            }

            return Directory.GetFiles(this.FullPath)
                .Select(f => f.AsFileSystemPath())
                .ToArray();
        }

        public bool IsDirectory()
        {
            return Directory.Exists(this.FullPath);
        }

        public bool IsFile()
        {
            return File.Exists(this.FullPath);
        }

        public IFileSystemPath Parent()
        {
            return new FileSystemPath(Path.GetDirectoryName(this.FullPath));
        }

        public T ReadJsonFile<T>()
        {
            return JsonConvert.DeserializeObject<T>(this.ReadAllText());
        }

        public string ReadAllText()
        {
            if (!this.IsFile())
            {
                throw new FileNotFoundException($"File '{this.FullPath}' could not be found");
            }

            return File.ReadAllText(this.FullPath);
        }

        public override bool Equals(object obj)
        {
            return this.FullPath.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.FullPath.GetHashCode();
        }

        public string GetExtension()
        {
            return Path.GetExtension(this.FullPath);
        }

        public string GetFileName()
        {
            return Path.GetFileName(this.FullPath);
        }

        public string GetFileNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(this.FullPath);
        }
    }
}
