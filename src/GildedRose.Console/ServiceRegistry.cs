using GildedRose.Console.Commands;
using GildedRose.Console.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GildedRose.Console;

static class ServiceRegistry
{
    public static IServiceCollection RegisterConsoleServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IController, Controller>()
            .AddTransient<ICommand, ListCommand>()
            .AddTransient<ICommand, DetailsCommand>()
            .AddTransient<ICommand, QuitCommand>()
            .AddTransient<ICommand, EndDayCommand>()
            .AddTransient<ICommand, GetTrashCommand>();
    }
}
