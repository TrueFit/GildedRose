using System;
using System.IO;

namespace gild_repo
{
    /// <summary>
    /// Wraps some of the functionality of FileSystemInfo so that it can be used more easily with DI.
    /// </summary>
    public interface IFileSystemInfoWrapper
    {
        DateTime CreationTimeUtc { get; }
        string FullName { get; }
    }

    public class FileSystemInfoWrapper : IFileSystemInfoWrapper
    {
        private readonly FileSystemInfo _fileSystemInfo;

        public FileSystemInfoWrapper(FileSystemInfo fileSystemInfo)
        {
            _fileSystemInfo = fileSystemInfo;
        }

        public DateTime CreationTimeUtc
        {
            get { return _fileSystemInfo.CreationTime; }
        }

        public string FullName
        {
            get { return _fileSystemInfo.FullName; }
        }
    }
}
