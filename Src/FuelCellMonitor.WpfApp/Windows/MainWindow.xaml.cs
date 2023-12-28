using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class MainWindow : Window
{
    private readonly ISerialModbusClientService _modbus;
    private readonly ISerialModbusDataScalingService _modbusScaling;

    public MainWindow(
        ISerialModbusClientService modbus,
        ISerialModbusDataScalingService scalingService)
    {
        InitializeComponent();

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
}