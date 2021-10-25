using GildedRose.Console.Interfaces;
using GildedRose.Inventory.Interfaces;

namespace GildedRose.Console.Commands;

class DetailsCommand : BaseCommand
{
    public DetailsCommand(IInventoryList inventory) : base(inventory) { }

    public override string Name => "Details";

    public override string Execute(string args) => Inventory.GetItem(args).ToString();

    public override string Description => "Gets the inventory detail for the item name provided";
}
