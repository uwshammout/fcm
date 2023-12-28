using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class DeviceConnectionWindow : Window
{
    private readonly ISerialPortsDiscoveryService _serialPortsDiscovery;
    private readonly ISerialOptionsService _serialOptions;
    private readonly ISerialModbusClientService _serialModbusClient;

    public DeviceConnectionWindow(
        ISerialPortsDiscoveryService serialPortsDiscovery,
        ISerialOptionsService serialOptions,
        ISerialModbusClientService serialModbusClient)
    {
        InitializeComponent();

        _serialPortsDiscovery = serialPortsDiscovery;
        _serialOptions = serialOptions;
        _serialModbusClient = serialModbusClient;

        _serialPortsDiscovery.StartPortsDiscovery();
    }

    protected override void OnClosed(EventArgs e)
    {
        _serialPortsDiscovery.StopPortsDiscovery();
        base.OnClosed(e);
    }
}
