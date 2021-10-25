using GildedRose.Console.Interfaces;
using GildedRose.Inventory.Interfaces;

namespace GildedRose.Console.Commands;

class GetTrashCommand : BaseCommand
{
    public GetTrashCommand(IInventoryList inventory) : base(inventory) { }

    public override string Name => "Remove Trash";

    public override string Execute(string args) => Inventory.RemoveTrash();

    public override string Description => "Lists the current inventory to be disposed and removes it from inventory";
}
