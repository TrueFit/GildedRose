using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace gild_repo
{
    public interface IFileSystemWrapper
    {
        void CreateDirectoryIfItDoesntExist(string directory);

        List<IFileSystemInfoWrapper> GetFileInfos(string directory, string searchPattern);

        void WriteAllText(string fileName, string contents);

        string ReadAllText(string fileName);
    }

    public class FileSystemWrapper : IFileSystemWrapper
    {
        public void CreateDirectoryIfItDoesntExist(string directory)
        {
            var directoryInfo = new DirectoryInfo(directory);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }

        public List<IFileSystemInfoWrapper> GetFileInfos(string directory, string searchPattern)
        {
            var di = new DirectoryInfo(directory);
            if (!di.Exists) { return new List<IFileSystemInfoWrapper>(); }

            return di.GetFiles(searchPattern).Select(item => (IFileSystemInfoWrapper)new FileSystemInfoWrapper(item)).ToList();
        }

        public void WriteAllText(string fileName, string contents)
        {
            File.WriteAllText(fileName, contents);
        }

        public string ReadAllText(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}
