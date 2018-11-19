using System;
using System.Collections.Generic;
using System.Linq;
using gild_repo;
using Moq;

namespace gild_repo_tests
{
    public class InMemoryFileSystem : IFileSystemWrapper
    {
        private class FileData
        {
            public string Contents { get; set; }
            public DateTime CreationTimeUtc { get; set; }
        }

        private readonly List<string> _directories = new List<string>();
        private readonly Dictionary<string, FileData> _files = new Dictionary<string, FileData>();

        public void CreateDirectoryIfItDoesntExist(string directory)
        {
            if (!_directories.Contains(directory)) { _directories.Add(directory); }
        }

        public List<IFileSystemInfoWrapper> GetFileInfos(string directory, string searchPattern)
        {
            return _files.Keys.Select(fileName =>
            {
                var data = _files[fileName];

                var fileInfo = new Mock<IFileSystemInfoWrapper>();
                fileInfo.Setup(mock => mock.FullName).Returns(fileName);
                fileInfo.Setup(mock => mock.CreationTimeUtc).Returns(data.CreationTimeUtc);

                return fileInfo.Object;
            }).ToList();
        }

        public string ReadAllText(string fileName)
        {
            if (!_files.ContainsKey(fileName)) { throw new ApplicationException($"File \"{fileName}\"not found."); }
            return _files[fileName].Contents;
        }

        public void WriteAllText(string fileName, string contents)
        {
            _files[fileName] = new FileData
            {
                Contents = contents,
                CreationTimeUtc = DateTime.UtcNow
            };
        }
    }
}
