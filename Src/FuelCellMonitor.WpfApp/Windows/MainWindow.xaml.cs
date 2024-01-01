using CronBlocks.FuelCellMonitor.InternalServices;
using CronBlocks.FuelCellMonitor.Settings;
using CronBlocks.FuelCellMonitor.Startup;
using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

    private readonly Timer _timer;
    private DateTime _experimentStartTime = DateTime.MinValue;

    private Brush _fuelCellStartButtonInitialColor;
    private Brush _fuelCellSeriesStartButtonInitialColor;
    private Brush _electrolyzerStartButtonInitialColor;

    private string _fuelCellStartButtonInitialText;
    private string _fuelCellSeriesStartButtonInitialText;
    private string _electrolyzerStartButtonInitialText;
    private string _stopText = "STOP";

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

        _timer = new Timer(OnTimerTick, null, 500, 500);

        _fuelCellStartButtonInitialColor = FuelCellStartButton.Background;
        _fuelCellSeriesStartButtonInitialColor = FuelCellSeriesStartButton.Background;
        _electrolyzerStartButtonInitialColor = ElectrolyzerStartButton.Background;

        _fuelCellStartButtonInitialText = (string)FuelCellStartButton.Content;
        _fuelCellSeriesStartButtonInitialText = (string)FuelCellSeriesStartButton.Content;
        _electrolyzerStartButtonInitialText = (string)ElectrolyzerStartButton.Content;

        _plottingState = PlottingState.None;
        _lastPlottingState = PlottingState.None;
        ChangePlottingState(_plottingState);

        _modbus.OperationStateChanged += OnDeviceOperationStateChanged;
        OnDeviceOperationStateChanged(_modbus.OperationState);

        _modbusScaling.NewValuesReceived += OnNewValuesReceived;
    }

    protected override void OnClosed(EventArgs e)
    {
        _timer.Dispose();

        _dataExchange.Dispose();
        _modbusScaling.Dispose();
        _modbus.Dispose();

        base.OnClosed(e);
    }

    private void OnTimerTick(object? state)
    {
        Dispatcher.Invoke(() =>
        {
            switch (_plottingState)
            {
                case PlottingState.None: break;

                case PlottingState.FuelCell:
                case PlottingState.FuelCellSeries:
                case PlottingState.Electrolyzer:

                    double secondsElapsed = (DateTime.Now - _experimentStartTime).TotalSeconds;
                    double secondsTotal = _dataExchange.ExperimentTimeDuration;
                    double secondsRemaining = secondsTotal - secondsElapsed;

                    if (secondsElapsed >= _dataExchange.ExperimentTimeDuration)
                    {
                        ChangePlottingState(PlottingState.None);
                    }
                    else
                    {
                        DataProgress.Value = (secondsElapsed / secondsTotal) * 100;
                        DataProgressMessage.Text = $"{(int)secondsRemaining} Seconds Remaining";
                    }

                    break;
            }
        });
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

                case PlottingState.FuelCell:
                    {
                        FuelCellVIPlot.Update(fcTotalCurrent, fcTotalVoltage, 0, 0);
                        FuelCellPTPlot.Update(fcTotalPower, 0);
                        FuelCellPIPlot.Update(fcTotalCurrent, fcTotalPower, 0, 0);
                        FuelCellPVPlot.Update(fcTotalVoltage, fcTotalPower, 0, 0);

                        if (fcTotalVoltage > FuelCellVoltageGauge.Dial2Value)
                        {
                            FuelCellVoltageGauge.Dial2Value = fcTotalVoltage;
                        }

                        if (fcTotalCurrent > FuelCellCurrentGauge.Dial2Value)
                        {
                            FuelCellCurrentGauge.Dial2Value = fcTotalCurrent;
                        }

                        if (fcTotalPower > FuelCellPowerGauge.Dial2Value)
                        {
                            FuelCellPowerGauge.Dial2Value = fcTotalPower;

                            FuelCellVoltageGauge.Dial3Value = fcTotalVoltage;
                            FuelCellCurrentGauge.Dial3Value = fcTotalCurrent;
                            FuelCellPowerGauge.Dial3Value = fcTotalPower;
                        }
                    }
                    break;

                case PlottingState.FuelCellSeries:
                    {
                        FuelCellSeriesVIPlot.Update(fcTotalCurrent, fcTotalVoltage, 0, 0);
                        FuelCellSeriesPTPlot.Update(fcTotalPower, 0);

                        if (fcTotalVoltage > FuelCellSeriesVoltageGauge.Dial2Value)
                        {
                            FuelCellSeriesVoltageGauge.Dial2Value = fcTotalVoltage;
                        }

                        if (fcTotalCurrent > FuelCellSeriesCurrentGauge.Dial2Value)
                        {
                            FuelCellSeriesCurrentGauge.Dial2Value = fcTotalCurrent;
                        }

                        if (fcTotalPower > FuelCellSeriesPowerGauge.Dial2Value)
                        {
                            FuelCellSeriesPowerGauge.Dial2Value = fcTotalPower;

                            FuelCellSeriesVoltageGauge.Dial3Value = fcTotalVoltage;
                            FuelCellSeriesCurrentGauge.Dial3Value = fcTotalCurrent;
                            FuelCellSeriesPowerGauge.Dial3Value = fcTotalPower;
                        }
                    }
                    break;
                case PlottingState.Electrolyzer:
                    {
                        ElectrolyzerIVPlot.Update(elTotalVoltage, elTotalCurrent, 0, 0);

                        if (elTotalVoltage > ElectrolyzerVoltageGauge.Dial2Value)
                        {
                            ElectrolyzerVoltageGauge.Dial2Value = elTotalVoltage;
                        }

                        if (elTotalCurrent > ElectrolyzerCurrentGauge.Dial2Value)
                        {
                            ElectrolyzerCurrentGauge.Dial2Value = elTotalCurrent;
                        }

                        if (elTotalPower > ElectrolyzerPowerGauge.Dial2Value)
                        {
                            ElectrolyzerPowerGauge.Dial2Value = elTotalPower;

                            ElectrolyzerVoltageGauge.Dial3Value = elTotalVoltage;
                            ElectrolyzerCurrentGauge.Dial3Value = elTotalCurrent;
                            ElectrolyzerPowerGauge.Dial3Value = elTotalPower;
                        }
                    }
                    break;
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

                    FuelCellStartButton.IsEnabled = true;
                    FuelCellSeriesStartButton.IsEnabled = true;
                    ElectrolyzerStartButton.IsEnabled = true;
                    break;

                case OperationState.Stopped:
                    MessageBar.Text = "Device not connected";

                    FuelCellStartButton.IsEnabled = false;
                    FuelCellSeriesStartButton.IsEnabled = false;
                    ElectrolyzerStartButton.IsEnabled = false;
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
                    DataProgress.Visibility = Visibility.Hidden;
                    DataProgressMessage.Visibility = Visibility.Hidden;

                    FuelCellTabItem.IsEnabled = true;
                    FuelCellSeriesTabItem.IsEnabled = true;
                    ElectrolyzerTabItem.IsEnabled = true;

                    FuelCellStartButton.Background = _fuelCellStartButtonInitialColor;
                    FuelCellSeriesStartButton.Background = _fuelCellSeriesStartButtonInitialColor;
                    ElectrolyzerStartButton.Background = _electrolyzerStartButtonInitialColor;

                    FuelCellStartButton.Content = _fuelCellStartButtonInitialText;
                    FuelCellSeriesStartButton.Content = _fuelCellSeriesStartButtonInitialText;
                    ElectrolyzerStartButton.Content = _electrolyzerStartButtonInitialText;
                    break;

                case PlottingState.FuelCell:
                    FuelCellTabItem.IsEnabled = true;
                    FuelCellSeriesTabItem.IsEnabled = false;
                    ElectrolyzerTabItem.IsEnabled = false;

                    FuelCellStartButton.Background = DisplayColors.DisconnectButtonBg;
                    FuelCellStartButton.Content = _stopText;

                    FuelCellVIPlot.ClearData();
                    FuelCellPTPlot.ClearData();
                    FuelCellPIPlot.ClearData();
                    FuelCellPVPlot.ClearData();

                    FuelCellVoltageGauge.Dial2Value = 0;
                    FuelCellVoltageGauge.Dial3Value = 0;
                    FuelCellCurrentGauge.Dial2Value = 0;
                    FuelCellCurrentGauge.Dial3Value = 0;
                    FuelCellPowerGauge.Dial2Value = 0;
                    FuelCellPowerGauge.Dial3Value = 0;
                    break;

                case PlottingState.FuelCellSeries:
                    FuelCellTabItem.IsEnabled = false;
                    FuelCellSeriesTabItem.IsEnabled = true;
                    ElectrolyzerTabItem.IsEnabled = false;

                    FuelCellSeriesStartButton.Background = DisplayColors.DisconnectButtonBg;
                    FuelCellSeriesStartButton.Content = _stopText;

                    FuelCellSeriesVIPlot.ClearData();
                    FuelCellSeriesPTPlot.ClearData();

                    FuelCellSeriesVoltageGauge.Dial2Value = 0;
                    FuelCellSeriesVoltageGauge.Dial3Value = 0;
                    FuelCellSeriesCurrentGauge.Dial2Value = 0;
                    FuelCellSeriesCurrentGauge.Dial3Value = 0;
                    FuelCellSeriesPowerGauge.Dial2Value = 0;
                    FuelCellSeriesPowerGauge.Dial3Value = 0;
                    break;

                case PlottingState.Electrolyzer:
                    FuelCellTabItem.IsEnabled = false;
                    FuelCellSeriesTabItem.IsEnabled = false;
                    ElectrolyzerTabItem.IsEnabled = true;

                    ElectrolyzerStartButton.Background = DisplayColors.DisconnectButtonBg;
                    ElectrolyzerStartButton.Content = _stopText;

                    ElectrolyzerIVPlot.ClearData();

                    ElectrolyzerVoltageGauge.Dial2Value = 0;
                    ElectrolyzerVoltageGauge.Dial3Value = 0;
                    ElectrolyzerCurrentGauge.Dial2Value = 0;
                    ElectrolyzerCurrentGauge.Dial3Value = 0;
                    ElectrolyzerPowerGauge.Dial2Value = 0;
                    ElectrolyzerPowerGauge.Dial3Value = 0;
                    break;
            }

            if (state != PlottingState.None)
            {
                DataProgress.Visibility = Visibility.Visible;
                DataProgressMessage.Visibility = Visibility.Visible;

                DataProgress.Value = 0;

                _experimentStartTime = DateTime.Now;
            }

            _lastPlottingState = _plottingState;
            _plottingState = state;
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

    private void OnStartButtonClicked(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        bool doStop = ((string)button.Content) == _stopText;

        if (doStop)
        {
            ChangePlottingState(PlottingState.None);
        }
        else
        {
            if (button == FuelCellStartButton)
            {
                ChangePlottingState(PlottingState.FuelCell);
            }
            else if (button == FuelCellSeriesStartButton)
            {
                ChangePlottingState(PlottingState.FuelCellSeries);
            }
            else if (button == ElectrolyzerStartButton)
            {
                ChangePlottingState(PlottingState.Electrolyzer);
            }
        }
    }
}