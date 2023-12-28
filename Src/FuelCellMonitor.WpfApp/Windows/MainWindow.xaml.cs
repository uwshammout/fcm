using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class MainWindow : Window
{
    private readonly ISerialPortsDiscoveryService _portsDiscovery;
    private readonly ISerialModbusClientService _modbus;
    private readonly ISerialOptionsService _serialOptions;
    private readonly ISerialModbusDataScalingService _modbusScaling;

    public MainWindow(
        ISerialPortsDiscoveryService portsDiscovery,
        ISerialModbusClientService modbus,
        ISerialOptionsService serialOptions,
        ISerialModbusDataScalingService scalingService)
    {
        InitializeComponent();

        _portsDiscovery = portsDiscovery;
        _modbus = modbus;
        _serialOptions = serialOptions;
        _modbusScaling = scalingService;
    }

    protected override void OnClosed(EventArgs e)
    {
        _modbusScaling.Dispose();
        _modbus.Dispose();

        base.OnClosed(e);
    }
}