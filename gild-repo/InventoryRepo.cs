using gild_model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace gild_repo
{
    public interface IInventoryRepo : IEventRepo
    {
        List<InventoryItem> Get(FileSystemConnectionContext connectionContext);
        List<InventoryItem> GetTrash(FileSystemConnectionContext connectionContext);
        void InitializeData(FileSystemConnectionContext connectionContext);
    }

    public class InventoryRepo: EventRepo, IInventoryRepo
    {
        private readonly IResourceRepo _resourceRepo;
        private readonly IInventorySnapshotProcessor _inventorySnapshotProcessor;

        private readonly IFileSystemWrapper _fileSystem;

        public InventoryRepo(
            IInventorySnapshotProcessor snapshotProcessor,
            IResourceRepo resourceRepo,
            IFileSystemWrapper fileSystem)
            : base(fileSystem)
        {
            _resourceRepo = resourceRepo;
            _fileSystem = fileSystem;

            _inventorySnapshotProcessor = snapshotProcessor;
        }

        public List<InventoryItem> Get(FileSystemConnectionContext connectionContext)
        {
            var snapshot = ProcessEvents(connectionContext, _inventorySnapshotProcessor, InitializeData);

            return snapshot?.InventoryItems ?? new List<InventoryItem>();
        }

        public List<InventoryItem> GetTrash(FileSystemConnectionContext connectionContext)
        {
            var snapshot = ProcessEvents(connectionContext, _inventorySnapshotProcessor, InitializeData);

            return (snapshot?.InventoryItems ?? new List<InventoryItem>())
                .Where(item => item.Quality <= 0)
                .ToList();
        }
        
        public void InitializeData(FileSystemConnectionContext connectionContext)
        {
            Insert(connectionContext, new SetInitialInventoryDataEvent { InventoryItems = GetInitialInventory(connectionContext) });
        }

        private List<InventoryItem> GetInitialInventory(FileSystemConnectionContext connectionContext)
        {
            const string ResourceName = "gild_repo.res.inventory.txt";
            var inventoryText = _resourceRepo.GetResource(ResourceName);
            var lines = inventoryText.Replace("\r\n", "\r").Replace("\n", "\r").Split('\r')
                .Where(queryLine => !string.IsNullOrWhiteSpace(queryLine))
                .Select(queryLine => queryLine.Trim())
                .ToList();

            var initialGoods = lines.Select(queryLine =>
            {
                var pieces = queryLine.Split(',').ToList();

                // name, category, sell-in, quality
                const int ExpectedPieces = 4;
                if (pieces.Count != ExpectedPieces) { throw new ApplicationException($"Expected {ExpectedPieces} pieces, found {pieces.Count}."); }

                var name = pieces[0];
                var category = pieces[1];
                var sellIn = decimal.Parse(pieces[2]);
                var quality = decimal.Parse(pieces[3]);

                return new InventoryItem
                {
                    Name = name,
                    Category = category,
                    SellIn = sellIn,
                    Quality = quality
                };
            }).ToList();

            return initialGoods;
        }
    }
}
