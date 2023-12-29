using CronBlocks.FuelCellMonitor.Startup;
using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class MainWindow : Window
{
    private readonly App _app;
    private readonly ISerialModbusClientService _modbus;
    private readonly ISerialModbusDataScalingService _modbusScaling;

    public MainWindow(
        App app,
        ISerialModbusClientService modbus,
        ISerialModbusDataScalingService scalingService)
    {
        InitializeComponent();
        
        _app = app;
        _modbus = modbus;
        _modbusScaling = scalingService;

        _modbus.OperationStateChanged += OnDeviceOperationStateChanged;
        OnDeviceOperationStateChanged(_modbus.OperationState);

        _modbusScaling.NewValuesReceived += OnNewValuesReceived;
    }

    protected override void OnClosed(EventArgs e)
    {
        _modbusScaling.Dispose();
        _modbus.Dispose();

        base.OnClosed(e);
    }

    private void OnNewValuesReceived(List<double> values)
    {
    }

    private void OnDeviceOperationStateChanged(OperationState state)
    {
        Dispatcher.Invoke(() =>
        {
            switch (state)
            {
                case OperationState.Running:
                    MessageBar.Text = "Device connected";
                    break;

                case OperationState.Stopped:
                    MessageBar.Text = "Device not connected";
                    DataProgress.Visibility = Visibility.Hidden;
                    break;
            }
        });
    }

    private void OnMenuItemClicked(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem mi)
        {
            string? header = mi.Header as string;

            if (string.IsNullOrEmpty(header) == false)
            {
                Window window = null!;

                switch (header)
                {
                    case "Connect / Disconnect":
                        window = _app.GetInstance<DeviceConnectionWindow>();                        
                        break;

                    case "Calibrate":
                        window = _app.GetInstance<DeviceCalibrationWindow>();
                        break;
                }

                if (window != null)
                {
                    window.ShowDialog();
                }
            }
        }
        
    }
}