using CronBlocks.FuelCellMonitor.Settings;
using CronBlocks.Helpers.Extensions;
using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Extensions;
using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class DeviceConnectionWindow : Window
{
    private readonly ISerialPortsDiscoveryService _ports;
    private readonly ISerialOptionsService _options;
    private readonly ISerialModbusClientService _modbus;

    private SerialModbusClientSettings _settings;

    private List<FrameworkElement> _allSelectionElements;

    private readonly Brush DeviceAddressInputBgBrush;
    private readonly Brush RegistersStartAddressInputBgBrush;

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

        DeviceAddressInputBgBrush = DeviceAddressInput.Background;
        RegistersStartAddressInputBgBrush = RegistersStartAddressHeading.Background;

        _ports = serialPortsDiscovery;
        _options = serialOptions;
        _modbus = serialModbusClient;

        _settings = _modbus.GetComSettings();

        _modbus.OperationStateChanged += HandleModbusStatus;
        HandleModbusStatus(_modbus.OperationState);
    }

    protected override void OnClosed(EventArgs e)
    {
        _modbus.OperationStateChanged -= HandleModbusStatus;

        _ports.NewPortFound -= OnComPortFound;
        _ports.ExistingPortRemoved -= OnComPortRemoved;
        _ports.StopPortsDiscovery();

        base.OnClosed(e);
    }

    private void HandleModbusStatus(OperationState status)
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

                    _settings = _modbus.GetComSettings();

                    PortInput.Items.Add(_settings.ComPort);
                    BaudRateInput.Items.Add(_settings.BaudRate.ToDisplayString());
                    DataBitsInput.Items.Add(_settings.DataBits.ToDisplayString());
                    ParityInput.Items.Add(_settings.Parity.ToDisplayString());
                    StopBitsInput.Items.Add(_settings.StopBits.ToDisplayString());
                    DeviceAddressInput.Text = _settings.DeviceAddress.ToString();
                    RegistersStartAddressInput.Text = _settings.RegistersStartAddressHexStr;

                    PortInput.SelectedIndex = 0;
                    BaudRateInput.SelectedIndex = 0;
                    DataBitsInput.SelectedIndex = 0;
                    ParityInput.SelectedIndex = 0;
                    StopBitsInput.SelectedIndex = 0;

                    break;

                case OperationState.Stopped:
                    EnableSelectionControls(true);

                    _settings = _modbus.GetComSettings();

                    int index;

                    index = 0;
                    foreach (string option in _options.GetAllOptions<BaudRate>())
                    {
                        BaudRateInput.Items.Add(option);
                        if (option == _settings.BaudRate.ToDisplayString())
                        {
                            BaudRateInput.SelectedIndex = index;
                        }
                        index++;
                    }

                    index = 0;
                    foreach (string option in _options.GetAllOptions<DataBits>())
                    {
                        DataBitsInput.Items.Add(option);
                        if (option == _settings.DataBits.ToDisplayString())
                        {
                            DataBitsInput.SelectedIndex = index;
                        }
                        index++;
                    }

                    index = 0;
                    foreach (string option in _options.GetAllOptions<Parity>())
                    {
                        ParityInput.Items.Add(option);
                        if (option == _settings.Parity.ToDisplayString())
                        {
                            ParityInput.SelectedIndex = index;
                        }
                        index++;
                    }

                    index = 0;
                    foreach (string option in _options.GetAllOptions<StopBits>())
                    {
                        StopBitsInput.Items.Add(option);
                        if (option == _settings.StopBits.ToDisplayString())
                        {
                            StopBitsInput.SelectedIndex = index;
                        }
                        index++;
                    }

                    DeviceAddressInput.Text = _settings.DeviceAddress.ToString();
                    RegistersStartAddressInput.Text = _settings.RegistersStartAddressHexStr;

                    _ports.NewPortFound += OnComPortFound;
                    _ports.ExistingPortRemoved += OnComPortRemoved;
                    _ports.StartPortsDiscovery();

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

    private void OnInputSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string name = ((ComboBox)sender).Name;

        switch (name)
        {
            case "PortInput": break;
            case "BaudRateInput": break;
            case "DataBitsInput": break;
            case "ParityInput": break;
            case "StopBitsInput": break;

            default:
                throw new NotImplementedException(
                $"ComboBox-'{name}' - {nameof(OnInputSelectionChanged)} is not implemented");
        }
    }

    private void OnInputTextChanged(object sender, TextChangedEventArgs e)
    {
        string name = ((TextBox)sender).Name;

        switch (name)
        {
            case "DeviceAddressInput":
                if (DeviceAddressInput.Text.IsValidDeviceAddress())
                {
                    DeviceAddressInput.Background = DeviceAddressInputBgBrush;
                    _settings.DeviceAddress = int.Parse(DeviceAddressInput.Text);
                }
                else
                {
                    DeviceAddressInput.Background = DisplayColors.ErrorBg;
                }
                break;

            case "RegistersStartAddressInput":
                if (RegistersStartAddressInput.Text.IsValidHex())
                {
                    RegistersStartAddressInput.Background = RegistersStartAddressInputBgBrush;
                    _settings.RegistersStartAddressHexStr = RegistersStartAddressInput.Text;
                }
                else
                {
                    RegistersStartAddressInput.Background = DisplayColors.ErrorBg;
                }
                break;

            default:
                throw new NotImplementedException(
                $"TextBox-'{name}' - {nameof(OnInputTextChanged)} is not implemented");
        }
    }

    private void ConnectButtonClicked(object sender, RoutedEventArgs e)
    {

    }
}
