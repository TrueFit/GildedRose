using gild_model;
using System.Linq;

namespace gild_repo
{
    public interface IDayRepo : IEventRepo
    {
        void AdvanceDay(FileSystemConnectionContext connectionContext);
        int GetCurrentDay(FileSystemConnectionContext connectionContext);
    }

    public class DayRepo : EventRepo, IDayRepo
    {
        private readonly IFileSystemWrapper _fileSystemWrapper;

        public DayRepo(IFileSystemWrapper fileSystemWrapper)
            : base(fileSystemWrapper)
        {
            _fileSystemWrapper = fileSystemWrapper;
        }

        public void AdvanceDay(FileSystemConnectionContext connectionContext)
        {
            Insert(connectionContext, new AdvanceDayDataEvent());
        }

        public int GetCurrentDay(FileSystemConnectionContext connectionContext)
        {
            var files = _fileSystemWrapper.GetFileInfos(connectionContext.DataRoot, "event--advance-day-*.json");
            return files != null ? files.Count : 0;
        }
    }
}
