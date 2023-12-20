using CronBlocks.SerialPortInterface.Interfaces;
using CronBlocks.SerialPortInterface.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CronBlocks.FuelCellMonitor.Startup;

internal static class ConfigureServices
{
    public static void ConfigureAppServices(this IServiceCollection services, App app)
    {
        services.AddSingleton((_) => app);
        services.AddSingleton((_) => services);
        services.AddSingleton<IPortsDiscoveryService, PortsDiscoveryService>();
        services.AddSingleton<IModbusAcquisitionService, ModbusAcquisitionService>();
        services.AddSingleton<Windows.MainWindow>();
    }
}
