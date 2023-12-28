using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class DeviceCallibrationWindow : Window
{
    private readonly ISerialModbusClientService _modbus;
    private readonly ISerialModbusDataScalingService _modbusScaling;

    public DeviceCallibrationWindow(
        ISerialModbusClientService modbus,
        ISerialModbusDataScalingService modbusScaling)
    {
        InitializeComponent();

        _modbus = modbus;
        _modbusScaling = modbusScaling;
    }
}
