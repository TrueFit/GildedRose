namespace GildedRose.Inventory.Domain;

class InventoryList : IInventoryList
{
    private List<IInventoryItem> _items;
    private readonly ICalculateQualityFactory _qualityFactory;

    public InventoryList(ICalculateQualityFactory qualityFactory)
    {
        _qualityFactory = qualityFactory;
        _items = new List<IInventoryItem>();

        foreach (var line in System.IO.File.ReadLines(@"inventory.txt"))
        {
            AddItem(line);
        }
    }

    public InventoryList(ICalculateQualityFactory qualityFactory, List<IInventoryItem> items)
    {
        _qualityFactory = qualityFactory;
        _items = items;
    }

    private void AddItem(string item)
    {
        var parts = item.Split(",");

        _items.Add(new InventoryItem(_qualityFactory)
        {
            Name = parts[0],
            Category = parts[1],        
            SellIn = int.Parse(parts[2]),
            Quality = int.Parse(parts[3])
        });
    }

    public string GetItem(string name)
    {
        return _items?.FirstOrDefault(i => i.Name == name)?.ToString() ??
            $"The item {name} was not found";
    }

    private Predicate<IInventoryItem> TrashItems => (i => i.Quality == 0);

    public void EndDay()
    {
        _items?.ForEach(i => i.EndDay());
    }

    public string RemoveTrash()
    {
        var message = String.Join(Environment.NewLine,
            _items?.Where(i => TrashItems(i)));

        _items.RemoveAll(TrashItems);

        return message;
    }

    public override string ToString()
    {
        return String.Join(Environment.NewLine, _items);
    }
}
