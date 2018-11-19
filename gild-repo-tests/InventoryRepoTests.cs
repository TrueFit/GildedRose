using gild_repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

namespace gild_repo_tests
{
    [TestClass]
    public class InventoryRepoTests
    {
        private InventoryRepo _inventoryRepo;
        private FileSystemConnectionContext _connectionContext;

        [TestInitialize]
        public void Setup()
        {
            _connectionContext = new FileSystemConnectionContext { DataRoot = @"TEST:\ThisIsNotARealDirectory\" };

            var snapshotProcessor = new Mock<IInventorySnapshotProcessor>();
            var resourceRepo = new ResourceRepo();

            var fileSystemWrapper = new InMemoryFileSystem();

            _inventoryRepo = new InventoryRepo(snapshotProcessor.Object, resourceRepo, fileSystemWrapper);
        }

        [TestMethod]
        public void Inventory_repo__a_new_repo_should_get_the_initial_inventory_set()
        {
            var results = _inventoryRepo.Get(_connectionContext);
            results.Count.ShouldBe(20);
        }
    }
}
