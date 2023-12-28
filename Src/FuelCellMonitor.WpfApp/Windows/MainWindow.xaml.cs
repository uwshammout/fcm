using CronBlocks.FuelCellMonitor.Startup;
using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

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
        _modbusScaling.NewValuesReceived += OnNewValuesReceived;
    }

    private void OnNewValuesReceived(List<double> values)
    {
    }

    private void OnDeviceOperationStateChanged(OperationState state)
    {
    }

    protected override void OnClosed(EventArgs e)
    {
        _modbusScaling.Dispose();
        _modbus.Dispose();

        base.OnClosed(e);
    }

    private void On_MenuItem_Device_ConnectDisconnect(object sender, RoutedEventArgs e)
    {
        var w = _app.GetInstance<DeviceConnectionWindow>();
        w.ShowDialog();
    }

    private void On_MenuItem_Device_Callibrate(object sender, RoutedEventArgs e)
    {
        var w = _app.GetInstance<DeviceCallibrationWindow>();
        w.ShowDialog();
    }
}