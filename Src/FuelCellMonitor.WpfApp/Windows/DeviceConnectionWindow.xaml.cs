using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class DeviceConnectionWindow : Window
{
    private readonly ISerialPortsDiscoveryService _ports;
    private readonly ISerialOptionsService _options;
    private readonly ISerialModbusClientService _modbus;

    public DeviceConnectionWindow(
        ISerialPortsDiscoveryService serialPortsDiscovery,
        ISerialOptionsService serialOptions,
        ISerialModbusClientService serialModbusClient)
    {
        InitializeComponent();

        _ports = serialPortsDiscovery;
        _options = serialOptions;
        _modbus = serialModbusClient;

        _modbus.OperationStateChanged += OnModbusStatusChanged;
        OnModbusStatusChanged(_modbus.OperationState);
    }

    protected override void OnClosed(EventArgs e)
    {
        _modbus.OperationStateChanged -= OnModbusStatusChanged;

        _ports.NewPortFound -= OnComPortFound;
        _ports.ExistingPortRemoved -= OnComPortRemoved;
        _ports.StopPortsDiscovery();

        base.OnClosed(e);
    }

    private void OnModbusStatusChanged(OperationState status)
    {
        switch (status)
        {
            case OperationState.Running:
                break;

            case OperationState.Stopped:
                _ports.NewPortFound += OnComPortFound;
                _ports.ExistingPortRemoved += OnComPortRemoved;
                _ports.StartPortsDiscovery();
                break;
        }
    }

    private void OnComPortFound(string port)
    {
    }

    private void OnComPortRemoved(string port)
    {
    }

    private void ConnectButtonClicked(object sender, RoutedEventArgs e)
    {

    }
}
