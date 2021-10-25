using GildedRose.Console.Interfaces;
using GildedRose.Inventory.Interfaces;

namespace GildedRose.Console.Commands;

class QuitCommand : BaseCommand
{
    public QuitCommand(IInventoryList inventory) : base(inventory) { }

    public override string Name => "Quit";

    public override string Execute(string args) => "Good Bye, Allison";

    public override string Description => "Quits the program";

    public override bool IsContinued => false;
}
