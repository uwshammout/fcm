using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class DeviceCalibrationWindow : Window
{
    private readonly ISerialModbusClientService _modbus;
    private readonly ISerialModbusDataScalingService _modbusScaling;

    public DeviceCalibrationWindow(
        ISerialModbusClientService modbus,
        ISerialModbusDataScalingService modbusScaling)
    {
        InitializeComponent();

        _modbus = modbus;
        _modbusScaling = modbusScaling;
    }
}
