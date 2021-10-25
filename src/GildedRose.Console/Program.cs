using GildedRose.Console;
using GildedRose.Console.Interfaces;
using GildedRose.Inventory.Services;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, Allison.");

Console.WriteLine("Welcome to the inventory system.");

var services = new ServiceCollection()
    .RegisterConsoleServices()
    .RegisterInventoryServices()
    .BuildServiceProvider();

var controller = services.GetRequiredService<IController>();

controller.Start();
