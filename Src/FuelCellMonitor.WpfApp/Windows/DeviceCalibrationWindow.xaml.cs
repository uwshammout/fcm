using CronBlocks.SerialPortInterface.Entities;
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

        _modbus.OperationStateChanged += OnDeviceOperationStateChanged;
        OnDeviceOperationStateChanged(_modbus.OperationState);

        _modbusScaling.NewValuesReceived += OnNewValuesReceived;
    }

    private void OnNewValuesReceived(List<double> list)
    {
    }

    private void OnDeviceOperationStateChanged(OperationState state)
    {
        Dispatcher.Invoke(() =>
        {
            switch (state)
            {
                case OperationState.Running:
                    StatusMessage.Text =
                        "Device connected -" +
                        " Dynamic updates available";
                    break;

                case OperationState.Stopped:
                    StatusMessage.Text =
                        "Device not connected -" +
                        " dynamic updates not available";

                    break;
            }
        });
    }

    private void OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {

    }

    protected override void OnClosed(EventArgs e)
    {
        _modbusScaling.SaveValuesToFile();
        base.OnClosed(e);
    }
}
