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
        if (values.Count < CronBlocks.SerialPortInterface.Configuration.Constants.TotalRegisters)
        {
            throw new InvalidOperationException(
                $"Expected number of values is {CronBlocks.SerialPortInterface.Configuration.Constants.TotalRegisters}" +
                $" whereas, {values.Count} values are received");
        }

        Dispatcher.Invoke(() =>
        {
            //- Fuel Cell

            double fcTotalVoltage = values[0];
            double fcTotalCurrent = values[1];
            double fcTotalPower = fcTotalVoltage * fcTotalCurrent;
            double fcC1Voltage = values[2];  //- First cell near ground
            double fcC2Voltage = values[3];  //- 
            double fcC3Voltage = values[4];  //- 
            double fcC4Voltage = values[5];  //- 
            double fcC5Voltage = values[6];  //- 
            double fcC6Voltage = values[7];  //- 
            double fcC7Voltage = values[8];  //- 
            double fcC8Voltage = values[9];  //- 
            double fcC9Voltage = values[10]; //- Last cell near the total voltage

            FuelCellVoltageGauge.Dial1Value = fcTotalVoltage;
            FuelCellCurrentGauge.Dial1Value = fcTotalCurrent;
            FuelCellPowerGauge.Dial1Value = fcTotalPower;

            //- Electrolyzer

            double elTotalVoltage = values[11];
            double elTotalCurrent = values[12];
            double elTotalPower = elTotalVoltage * elTotalCurrent;

            ElectrolyzerVoltageGauge.Dial1Value = elTotalVoltage;
            ElectrolyzerCurrentGauge.Dial1Value = elTotalCurrent;
            ElectrolyzerPowerGauge.Dial1Value = elTotalPower;
        });
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