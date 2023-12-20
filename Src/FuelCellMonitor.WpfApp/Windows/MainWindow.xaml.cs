using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class MainWindow : Window
{
    private readonly IPortsDiscoveryService portsDiscovery;
    private readonly IModbusAcquisitionService modbus;

    public MainWindow(
        IPortsDiscoveryService portsDiscovery,
        IModbusAcquisitionService modbus)
    {
        InitializeComponent();

        this.portsDiscovery = portsDiscovery;
        this.modbus = modbus;
    }
}