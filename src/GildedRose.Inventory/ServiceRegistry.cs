using System.Runtime.CompilerServices;
using GildedRose.Inventory.Domain;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("GildedRose.Inventory.Tests")]

namespace GildedRose.Inventory.Services;

public static class ServiceRegistry
{
    public static IServiceCollection RegisterInventoryServices(this IServiceCollection services)
    {
        return services
            .AddTransient<ICalculateQuality, AgedBrieQualityCalculator>()
            .AddTransient<ICalculateQuality, BackstagePassQualityCalculator>()
            .AddTransient<ICalculateQuality, ConjuredQualityCalculator>()
            .AddTransient<ICalculateQuality, DefaultQualityCalculator>()
            .AddTransient<ICalculateQuality, SulfurasQualityCalculator>()
            .AddTransient<ICalculateQualityFactory, CalculateQualityFactory>()
            .AddSingleton<IInventoryList, InventoryList>();
    }
}
