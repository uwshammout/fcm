using CronBlocks.FuelCellMonitor.InternalServices;
using CronBlocks.FuelCellMonitor.Settings;
using CronBlocks.Helpers;
using CronBlocks.SerialPortInterface.Interfaces;
using CronBlocks.SerialPortInterface.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CronBlocks.FuelCellMonitor.Startup;

internal static class ConfigureServices
{
    public static void ConfigureAppServices(this IServiceCollection services, App app)
    {
        //- Overall
        services.AddSingleton((_) => app);
        services.AddSingleton((_) => services);

        //- Serial Port Services
        services.AddSingleton<ISerialPortsDiscoveryService, SerialPortsDiscoveryService>();
        services.AddSingleton<ISerialModbusClientService, SerialModbusClientService>();
        services.AddSingleton<ISerialOptionsService, SerialOptionsService>();
        services.AddSingleton<ISerialModbusDataScalingService>((sp) =>
        {
            return new SerialModbusDataScalingService(
                sp.GetRequiredService<ILogger<SerialModbusDataScalingService>>(),
                sp.GetRequiredService<ISerialModbusClientService>(),
                FilePaths.DataScalingFilename,
                sp.GetRequiredService<ILogger<IniConfigIO>>());
        });
        services.AddSingleton((sp) =>
        {
            return new DataExchangeService(
                new IniConfigIO(
                    FilePaths.DataExchangeSvcFilename,
                    sp.GetRequiredService<ILogger<IniConfigIO>>()));
        });

        //- Windows
        services.AddSingleton<Windows.MainWindow>();
        services.AddTransient<Windows.DeviceConnectionWindow>();
        services.AddTransient<Windows.DeviceCalibrationWindow>((sp) =>
        {
            return new Windows.DeviceCalibrationWindow(
                sp.GetRequiredService<ISerialModbusClientService>(),
                sp.GetRequiredService<ISerialModbusDataScalingService>(),
                sp.GetRequiredService<DataExchangeService>(),
                new IniConfigIO(
                    FilePaths.PasswordFilename,
                    sp.GetRequiredService<ILogger<IniConfigIO>>())
                );
        });
        services.AddTransient<Windows.MeasurementSettingsWindow>();
    }
}
