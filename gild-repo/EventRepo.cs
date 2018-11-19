using gild_model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace gild_repo
{
    public interface IEventRepo
    {
        void Insert<TEntity>(FileSystemConnectionContext connectionString, TEntity model)
            where TEntity : DataEvent;
    }

    public abstract class EventRepo : IEventRepo
    {
        private readonly IFileSystemWrapper _fileSystem;

        public EventRepo(IFileSystemWrapper fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void Insert<TEntity>(FileSystemConnectionContext connectionContext, TEntity model)
            where TEntity : DataEvent
        {
            var fileTypeMarker = model is Snapshot
                ? "snapshot"
                : "event";

            var fullName = Path.Combine(connectionContext.DataRoot, $"{fileTypeMarker}--{model.ContractName}--{model.Id}.json");
            var contents = JsonConvert.SerializeObject(model, Formatting.Indented);

            _fileSystem.CreateDirectoryIfItDoesntExist(connectionContext.DataRoot);
            _fileSystem.WriteAllText(fullName, contents);
        }

        protected TSnapshot LoadSnapshot<TSnapshot>(FileSystemConnectionContext connectionContext)
            where TSnapshot : Snapshot
        {
            _fileSystem.CreateDirectoryIfItDoesntExist(connectionContext.DataRoot);

            var contractName = Activator.CreateInstance<TSnapshot>().ContractName;
            var info = _fileSystem.GetFileInfos(connectionContext.DataRoot, $"snapshot--{contractName}-*.json")
                .OrderByDescending(item => item.CreationTimeUtc)
                .FirstOrDefault();

            if (info != null)
            {
                var contents = _fileSystem.ReadAllText(info.FullName);
                return JsonConvert.DeserializeObject<TSnapshot>(contents);
            }

            return null;
        }

        protected TSnapshot ProcessEvents<TSnapshot>(
            FileSystemConnectionContext connectionContext,
            ISnapshotProcessor<TSnapshot> snapshotProcessor,
            Action<FileSystemConnectionContext> dataInitializer)
            where TSnapshot : Snapshot
        {
            _fileSystem.CreateDirectoryIfItDoesntExist(connectionContext.DataRoot);

            var loadInfos = new Func<List<IFileSystemInfoWrapper>>(() =>
                _fileSystem.GetFileInfos(connectionContext.DataRoot, "event-*.json")
                .OrderBy(item => item.CreationTimeUtc)
                .ToList());

            var infos = loadInfos();

            if (!infos.Any())
            {
                dataInitializer?.Invoke(connectionContext);
                infos = loadInfos();
            }

            var shouldSaveSnapshot = false;
            var snapshot = LoadSnapshot<TSnapshot>(connectionContext);
            if (snapshot == null)
            {
                // dataInitializer?.Invoke(connectionContext);
                snapshot = Activator.CreateInstance<TSnapshot>();
                shouldSaveSnapshot = true;
            }

            foreach (var info in infos)
            {
                if (snapshot.LastFileCreationTimeUtc.HasValue && snapshot.LastFileCreationTimeUtc >= info.CreationTimeUtc)
                { continue; }

                shouldSaveSnapshot = true;

                var contents = _fileSystem.ReadAllText(info.FullName);
                snapshotProcessor.ProcessEvent(snapshot, contents, info.CreationTimeUtc);
            }

            if (shouldSaveSnapshot)
            {
                Insert(connectionContext, snapshot);
            }

            return snapshot;
        }
    }
}
