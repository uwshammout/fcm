using Microsoft.Extensions.DependencyInjection;

namespace CronBlocks.FuelCellMonitor.Startup;

internal static class ConfigureServices
{
    public static void ConfigureAppServices(this IServiceCollection services)
    {
        services.AddSingleton<Windows.MainWindow>();
    }
}
