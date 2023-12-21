using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class MainWindow : Window
{
    private readonly ISerialPortsDiscoveryService portsDiscovery;
    private readonly ISerialModbusClientService modbus;
    private readonly ISerialOptionsService serialOptions;

    public MainWindow(
        ISerialPortsDiscoveryService portsDiscovery,
        ISerialModbusClientService modbus,
        ISerialOptionsService serialOptions)
    {
        InitializeComponent();

        this.portsDiscovery = portsDiscovery;
        this.modbus = modbus;
        this.serialOptions = serialOptions;
    }
}