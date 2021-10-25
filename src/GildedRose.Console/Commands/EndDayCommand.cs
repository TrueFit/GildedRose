using GildedRose.Console.Interfaces;
using GildedRose.Inventory.Interfaces;

namespace GildedRose.Console.Commands;

class EndDayCommand : BaseCommand
{
    public EndDayCommand(IInventoryList inventory) : base(inventory) { }

    public override string Name => "End Day";

    public override string Execute(string args)
    {
        Inventory.EndDay();

        return "The day has been ended.";
    }

    public override string Description => "Ages the inventory at the end of the day";
}
