using Microsoft.Extensions.DependencyInjection;

namespace CronBlocks.FuelCellMonitor.Startup;

internal static class ConfigureServices
{
    public static void ConfigureAppServices(this IServiceCollection services, App app)
    {
        services.AddSingleton<App>((_) => app);
        services.AddSingleton<IServiceCollection>((_) => services);
        services.AddSingleton<Windows.MainWindow>();
    }
}
