using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class MainWindow : Window
{
    private readonly ISerialPortsDiscoveryService portsDiscovery;
    private readonly ISerialModbusClientService modbus;

    public MainWindow(
        ISerialPortsDiscoveryService portsDiscovery,
        ISerialModbusClientService modbus)
    {
        InitializeComponent();

        this.portsDiscovery = portsDiscovery;
        this.modbus = modbus;
    }
}