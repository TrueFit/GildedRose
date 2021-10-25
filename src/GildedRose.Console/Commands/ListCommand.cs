using GildedRose.Console.Interfaces;
using GildedRose.Inventory.Interfaces;

namespace GildedRose.Console.Commands;

class ListCommand : BaseCommand
{
    public ListCommand(IInventoryList inventory) : base(inventory) { }

    public override string Name => "List";

    public override string Execute(string args) => Inventory.ToString();

    public override string Description => "Lists the current inventory for the Gilded Rose";
}
