using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class MainWindow : Window
{
    private readonly ISerialPortsDiscoveryService portsDiscovery;
    private readonly ISerialModbusClientService modbus;
    private readonly ISerialOptionsService serialOptions;
    private readonly ISerialModbusDataScalingService modbusScaling;

    public MainWindow(
        ISerialPortsDiscoveryService portsDiscovery,
        ISerialModbusClientService modbus,
        ISerialOptionsService serialOptions,
        ISerialModbusDataScalingService scalingService)
    {
        InitializeComponent();

        this.portsDiscovery = portsDiscovery;
        this.modbus = modbus;
        this.serialOptions = serialOptions;
        this.modbusScaling = scalingService;
    }
}