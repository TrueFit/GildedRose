using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GildedRose.Services;
using NUnit.Framework;
using ApprovalTests;
using ApprovalTests.Reporters;
using GildedRose.Data;

namespace GildedRose.Tests
{
    [UseReporter(typeof(VisualStudioReporter))]
    [TestFixture]
    public class Tests
    {
        private IDataService _dataService;

        [SetUp]
        public void Setup()
        {
            _dataService = new DataService();
        }

        [Test]
        public async Task TestDataServiceApproval()
        {
            //arrange
            var reader = await _dataService.DownloadData();

            //act
            var content = await reader.ReadToEndAsync();

            //assert
            Approvals.Verify(content);
        }

        [Test]
        public async Task CreateInventory()
        {
            //arrange
            var factory = ItemFactory.Instance;
            var inventoryService = new InventoryService(_dataService, factory);

            //act 
            var result = await inventoryService.GetInventory();

            //assert
            Approvals.VerifyAll(result.ToList(), "");
        }

    }
}