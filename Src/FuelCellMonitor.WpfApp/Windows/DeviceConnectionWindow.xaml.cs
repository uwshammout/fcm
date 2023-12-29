using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class DeviceConnectionWindow : Window
{
    private readonly ISerialPortsDiscoveryService _ports;
    private readonly ISerialOptionsService _options;
    private readonly ISerialModbusClientService _modbus;

    private readonly List<FrameworkElement> _allSelectionElements;

    public DeviceConnectionWindow(
        ISerialPortsDiscoveryService serialPortsDiscovery,
        ISerialOptionsService serialOptions,
        ISerialModbusClientService serialModbusClient)
    {
        InitializeComponent();

        _allSelectionElements = new List<FrameworkElement>()
        {
            PortHeading,
            BaudRateHeading,
            DataBitsHeading,
            ParityHeading,
            StopBitsHeading,
            DeviceAddressHeading,
            RegistersStartAddressHeading,
            PortInput,
            BaudRateInput,
            DataBitsInput,
            ParityInput,
            StopBitsInput,
            DeviceAddressInput,
            RegistersStartAddressInput,
        };

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
        Dispatcher.Invoke(() =>
        {
            PortInput.Items.Clear();
            BaudRateInput.Items.Clear();
            DataBitsInput.Items.Clear();
            ParityInput.Items.Clear();
            StopBitsInput.Items.Clear();
            DeviceAddressInput.Text = "";
            RegistersStartAddressInput.Text = "";

            switch (status)
            {
                case OperationState.Running:
                    _ports.NewPortFound -= OnComPortFound;
                    _ports.ExistingPortRemoved -= OnComPortRemoved;
                    _ports.StopPortsDiscovery();

                    EnableSelectionControls(false);
                    break;

                case OperationState.Stopped:
                    _ports.NewPortFound += OnComPortFound;
                    _ports.ExistingPortRemoved += OnComPortRemoved;
                    _ports.StartPortsDiscovery();

                    EnableSelectionControls(true);
                    break;
            }
        });
    }

    private void EnableSelectionControls(bool enable)
    {
        foreach (var item in _allSelectionElements)
        {
            item.IsEnabled = enable;
        }
    }

    private void OnComPortFound(string port)
    {
        if (_modbus.OperationState == OperationState.Running)
            return;

        Dispatcher.Invoke(() =>
        {
            if (PortInput.Items.Contains(port)) return;

            PortInput.Items.Add(port);

            if (PortInput.Items.Count == 1)
            {
                PortInput.SelectedIndex = 0;
            }
        });
    }

    private void OnComPortRemoved(string port)
    {
        if (_modbus.OperationState == OperationState.Running)
            return;

        Dispatcher.Invoke(() =>
        {
            if (PortInput.Items.Contains(port))
                PortInput.Items.Remove(port);
        });
    }

    private void ConnectButtonClicked(object sender, RoutedEventArgs e)
    {

    }
}
