using CronBlocks.FuelCellMonitor.InternalServices;
using CronBlocks.FuelCellMonitor.Startup;
using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class MainWindow : Window
{
    private enum PlottingState
    {
        None, FuelCell, FuelCellSeries, Electrolyzer
    }

    private readonly App _app;
    private readonly DataExchangeService _dataExchange;
    private readonly ISerialModbusClientService _modbus;
    private readonly ISerialModbusDataScalingService _modbusScaling;

    private PlottingState _plottingState;
    private PlottingState _lastPlottingState;

    public MainWindow(
        App app,
        DataExchangeService dataExchange,
        ISerialModbusClientService modbus,
        ISerialModbusDataScalingService scalingService)
    {
        InitializeComponent();

        _app = app;
        _dataExchange = dataExchange;
        _modbus = modbus;
        _modbusScaling = scalingService;

        _plottingState = PlottingState.None;
        _lastPlottingState = PlottingState.None;
        ChangePlottingState(_plottingState);

        _modbus.OperationStateChanged += OnDeviceOperationStateChanged;
        OnDeviceOperationStateChanged(_modbus.OperationState);

        _modbusScaling.NewValuesReceived += OnNewValuesReceived;
    }

    protected override void OnClosed(EventArgs e)
    {
        _dataExchange.Dispose();
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

            FuelCellSeriesVoltageGauge.Dial1Value = fcTotalVoltage;
            FuelCellSeriesCurrentGauge.Dial1Value = fcTotalCurrent;
            FuelCellSeriesPowerGauge.Dial1Value = fcTotalPower;

            FuelCellNo1Voltage.Value = fcC1Voltage;
            FuelCellNo2Voltage.Value = fcC2Voltage - fcC1Voltage;
            FuelCellNo3Voltage.Value = fcC3Voltage - fcC2Voltage;
            FuelCellNo4Voltage.Value = fcC4Voltage - fcC3Voltage;
            FuelCellNo5Voltage.Value = fcC5Voltage - fcC4Voltage;
            FuelCellNo6Voltage.Value = fcC6Voltage - fcC5Voltage;
            FuelCellNo7Voltage.Value = fcC7Voltage - fcC6Voltage;
            FuelCellNo8Voltage.Value = fcC8Voltage - fcC7Voltage;
            FuelCellNo9Voltage.Value = fcC9Voltage - fcC8Voltage;
            FuelCellNo10Voltage.Value = fcTotalVoltage - fcC9Voltage;

            //- Electrolyzer

            double elTotalVoltage = values[11];
            double elTotalCurrent = values[12];
            double elTotalPower = elTotalVoltage * elTotalCurrent;

            ElectrolyzerVoltageGauge.Dial1Value = elTotalVoltage;
            ElectrolyzerCurrentGauge.Dial1Value = elTotalCurrent;
            ElectrolyzerPowerGauge.Dial1Value = elTotalPower;

            //- Plotting

            switch (_plottingState)
            {
                case PlottingState.None: break;
                case PlottingState.FuelCell: break;
                case PlottingState.FuelCellSeries: break;
                case PlottingState.Electrolyzer: break;
            }
        });
    }

    private void OnDeviceOperationStateChanged(OperationState state)
    {
        ChangePlottingState(PlottingState.None);

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

    private void ChangePlottingState(PlottingState state)
    {
        Dispatcher.Invoke(() =>
        {
            //- Last state cleanup - if any

            switch (_lastPlottingState)
            {
                case PlottingState.None: break;
                case PlottingState.FuelCell: break;
                case PlottingState.FuelCellSeries: break;
                case PlottingState.Electrolyzer: break;
            }

            //- Current state setup

            switch (state)
            {
                case PlottingState.None:
                    FuelCellTabItem.IsEnabled = true;
                    FuelCellSeriesTabItem.IsEnabled = true;
                    ElectrolyzerTabItem.IsEnabled = true;
                    break;

                case PlottingState.FuelCell:
                    FuelCellTabItem.IsEnabled = true;
                    FuelCellSeriesTabItem.IsEnabled = false;
                    ElectrolyzerTabItem.IsEnabled = false;
                    break;

                case PlottingState.FuelCellSeries:
                    FuelCellTabItem.IsEnabled = false;
                    FuelCellSeriesTabItem.IsEnabled = true;
                    ElectrolyzerTabItem.IsEnabled = false;
                    break;

                case PlottingState.Electrolyzer:
                    FuelCellTabItem.IsEnabled = false;
                    FuelCellSeriesTabItem.IsEnabled = false;
                    ElectrolyzerTabItem.IsEnabled = true;
                    break;
            }

            _lastPlottingState = state;
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
                    case "Save CSV":
                        break;

                    case "Connect / Disconnect":
                        window = _app.GetInstance<DeviceConnectionWindow>();
                        break;

                    case "Measurement Settings":
                        window = _app.GetInstance<MeasurementSettingsWindow>();
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