﻿using CronBlocks.FuelCellMonitor.InternalServices;
using CronBlocks.FuelCellMonitor.Settings;
using CronBlocks.SerialPortInterface.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace CronBlocks.FuelCellMonitor.Windows;

public partial class MeasurementSettingsWindow : Window, INotifyPropertyChanged
{
    private readonly ISerialModbusClientService _modbus;
    private readonly DataExchangeService _dataExchange;

    private double _acquisitionIntervalMinimum;
    private double _acquisitionIntervalMaximum;
    private double _acquisitionIntervalValue;

    private double _experimentTimeMinimum;
    private double _experimentTimeMaximum;
    private double _experimentTimeValue;

    public MeasurementSettingsWindow(
        ISerialModbusClientService modbus,
        DataExchangeService dataExchange)
    {
        InitializeComponent();

        _modbus = modbus;
        _dataExchange = dataExchange;

        AcquisitionIntervalMinimum = CronBlocks.SerialPortInterface.Configuration.Constants.MinimumDataAcquisitionIntervalMS;
        AcquisitionIntervalMaximum = CronBlocks.SerialPortInterface.Configuration.Constants.MaximumDataAcquisitionIntervalMS;
        AcquisitionIntervalValue = _modbus.GetDataAcquisitionInterval();

        ExperimentTimeMinimum = ValueConstants.ExperimentTimeMinimumSec;
        ExperimentTimeMaximum = ValueConstants.ExperimentTimeMaximumSec;
        ExperimentTimeValue = _dataExchange.ExperimentTimeDuration;

        DataContext = this;
    }

    public double AcquisitionIntervalMinimum
    {
        get => _acquisitionIntervalMinimum;
        set
        {
            if (_acquisitionIntervalMinimum != value)
            {
                _acquisitionIntervalMinimum = value;
                NotifyPropertyChanged();
            }
        }
    }
    public double AcquisitionIntervalMaximum
    {
        get => _acquisitionIntervalMaximum;
        set
        {
            if (_acquisitionIntervalMaximum != value)
            {
                _acquisitionIntervalMaximum = value;
                NotifyPropertyChanged();
            }
        }
    }
    public double AcquisitionIntervalValue
    {
        get => _acquisitionIntervalValue;
        set
        {
            if (_acquisitionIntervalValue != value)
            {
                _acquisitionIntervalValue = value;
                NotifyPropertyChanged();
            }
        }
    }

    public double ExperimentTimeMinimum
    {
        get => _experimentTimeMinimum;
        set
        {
            if (_experimentTimeMinimum != value)
            {
                _experimentTimeMinimum = value;
                NotifyPropertyChanged();
            }
        }
    }
    public double ExperimentTimeMaximum
    {
        get => _experimentTimeMaximum;
        set
        {
            if (_experimentTimeMaximum != value)
            {
                _experimentTimeMaximum = value;
                NotifyPropertyChanged();
            }
        }
    }
    public double ExperimentTimeValue
    {
        get => _experimentTimeValue;
        set
        {
            if (_experimentTimeValue != value)
            {
                _experimentTimeValue = value;
                NotifyPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Slider slider = (Slider)sender;
        string name = slider.Name;

        switch (name)
        {
            case "AcquisitionInterval":
                _modbus.SetDataAcquisitionInterval(slider.Value);
                break;

            case "ExperimentTime":
                _dataExchange.ExperimentTimeDuration = slider.Value;
                break;
        }
    }
}
