using CronBlocks.FuelCellMonitor.Settings;
using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class DeviceCalibrationWindow : Window
{
    private readonly ISerialModbusClientService _modbus;
    private readonly ISerialModbusDataScalingService _modbusScaling;

    private readonly TextBox[] _mfInputs;
    private readonly TextBox[] _offInputs;
    private readonly TextBlock[] _outputs;

    private readonly Brush _originalTextBoxBg;

    public DeviceCalibrationWindow(
        ISerialModbusClientService modbus,
        ISerialModbusDataScalingService modbusScaling)
    {
        InitializeComponent();

        _originalTextBoxBg = MultiplicationFactorA1.Background;

        _mfInputs =
            [
                MultiplicationFactorA1,
                MultiplicationFactorA2,
                MultiplicationFactorA3,
                MultiplicationFactorA4,
                MultiplicationFactorA5,
                MultiplicationFactorA6,
                MultiplicationFactorA7,
                MultiplicationFactorA8,
                MultiplicationFactorA9,
                MultiplicationFactorA10,
                MultiplicationFactorA11,
                MultiplicationFactorA12,
                MultiplicationFactorA13,
                MultiplicationFactorA14,
                MultiplicationFactorA15,
                MultiplicationFactorA16
            ];

        _offInputs =
            [
                OffsetA1,
                OffsetA2,
                OffsetA3,
                OffsetA4,
                OffsetA5,
                OffsetA6,
                OffsetA7,
                OffsetA8,
                OffsetA9,
                OffsetA10,
                OffsetA11,
                OffsetA12,
                OffsetA13,
                OffsetA14,
                OffsetA15,
                OffsetA16
            ];

        _outputs =
            [
                ProcessedValueA1,
                ProcessedValueA2,
                ProcessedValueA3,
                ProcessedValueA4,
                ProcessedValueA5,
                ProcessedValueA6,
                ProcessedValueA7,
                ProcessedValueA8,
                ProcessedValueA9,
                ProcessedValueA10,
                ProcessedValueA11,
                ProcessedValueA12,
                ProcessedValueA13,
                ProcessedValueA14,
                ProcessedValueA15,
                ProcessedValueA16
            ];

        _modbus = modbus;
        _modbusScaling = modbusScaling;

        _modbus.OperationStateChanged += OnDeviceOperationStateChanged;
        OnDeviceOperationStateChanged(_modbus.OperationState);

        _modbusScaling.NewValuesReceived += OnNewValuesReceived;
    }

    private void OnNewValuesReceived(List<double> list)
    {
        Dispatcher.Invoke(() =>
        {
            for (int i = 0; i < Math.Min(list.Count, _outputs.Length); i++)
            {
                _outputs[i].Text = list[i].ToString("0.00");
            }
        });
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

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        string name = textBox.Name;

        int index = -1;
        string prefix = null!;

        string mfPrefix = "MultiplicationFactorA";
        string offPrefix = "OffsetA";

        //- Let's match prefix

        if (name.StartsWith(mfPrefix))
        {
            prefix = mfPrefix;
        }
        else if (name.StartsWith(offPrefix))
        {
            prefix = offPrefix;
        }
        else
        {
            throw new NotImplementedException(
                $"Implementation missing from {nameof(OnTextChanged)} for" +
                $" handling {name}");
        }

        //- Let's get the index

        if (prefix != null)
        {
            if (int.TryParse(name.Substring(prefix.Length), out index) &&
                index >= 1 && index <= 16)
            {
                index -= 1;
            }
            else
            {
                throw new NotImplementedException(
                    $"{nameof(OnTextChanged)}: cannot get valid index from {name}");
            }
        }

        //- Check the value

        if (double.TryParse(textBox.Text, out double value))
        {
            textBox.Background = _originalTextBoxBg;

            if (prefix == mfPrefix)
            {
                _modbusScaling.SetMultiplicationFactor(index, value);
            }
            else if (prefix == offPrefix)
            {
                _modbusScaling.SetOffset(index, value);
            }
        }
        else
        {
            textBox.Background = DisplayColors.ErrorBg;
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        _modbusScaling.SaveValuesToFile();
        base.OnClosed(e);
    }
}
