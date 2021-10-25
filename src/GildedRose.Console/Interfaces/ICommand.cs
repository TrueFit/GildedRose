namespace GildedRose.Console;

interface ICommand
{
    string Name { get; }
    string Execute(string args);
    string Description { get; }
    bool IsContinued { get; }
}
