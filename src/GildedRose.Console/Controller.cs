using GildedRose.Console;
using GildedRose.Console.Interfaces;
using GildedRose.Inventory.Interfaces;
using Microsoft.Extensions.DependencyInjection;

class Controller : IController
{
    private const string _help = "Help";
    private readonly List<ICommand> _commands;
    
    public Controller(IEnumerable<ICommand> commands)
    {
        _commands = commands.ToList<ICommand>();
    }

    public void Start()
    {
        var isContinued = true;

        while (isContinued)
        {
            var input = Console.ReadLine();

            var command = _commands.FirstOrDefault(c => input.StartsWith(c.Name));

            var arguments = input.Replace($"{command?.Name} ", string.Empty);

            var message = command?.Execute(arguments) ?? GetHelp();

            Console.WriteLine(Environment.NewLine + message + Environment.NewLine);

            isContinued = command?.IsContinued ?? true;
        }
    }

    private string GetHelp()
    {
        var description = String.Join(Environment.NewLine,
            _commands.OrderBy(c => c.Name)
            .Select(c => $"{c.Name.PadRight(15)}{c.Description}"));

        return $"The command you entered cannot be found.  Here is a list of valid commands." +
            $"{Environment.NewLine + Environment.NewLine + description + Environment.NewLine + Environment.NewLine}" +
            $"Commands are case sensitive.";
    }
}
