using gild_repo;
using StructureMap;

namespace gild_api.DI
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IFileSystemWrapper>().Use<FileSystemWrapper>();

            For<IDayRepo>().Use<DayRepo>();
            For<IInventoryRepo>().Use<InventoryRepo>();

            For<IInventorySnapshotProcessor>().Use<InventorySnapshotProcessor>();
            For<IResourceRepo>().Use<ResourceRepo>();
        }
    }
}