﻿using CronBlocks.SerialPortInterface.Configuration;
using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Interfaces;
using FluentModbus;
using Microsoft.Extensions.Logging;

namespace CronBlocks.SerialPortInterface.Services;

public class ModbusAcquisitionService : IModbusAcquisitionService
{
    public event Action<List<double>>? NewValuesReceived;

    private readonly ILogger<ModbusAcquisitionService> _logger;

    //- Timer

    private bool _isRunning = false;
    private Timer _timer;

    //- Com Settings

    private bool _settingsProvided = false;

    private string _comPort = null!;

    private int _deviceAddress;
    private int _registersStartAddress;

    private BaudRate _baudRate = Constants.DefaultBaudRate;
    private DataBits _dataBits = Constants.DefaultDataBits;
    private Parity _parity = Constants.DefaultParity;
    private StopBits _stopBits = Constants.DefaultStopBits;

    //- MODBUS Client

    private ModbusRtuClient _client;
    
    //- Received Values

    private List<double> _valuesReceivedList;

    public ModbusAcquisitionService(ILogger<ModbusAcquisitionService> logger)
    {
        _isRunning = false;
        _timer = new Timer(AcquireData, null, TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

        _logger = logger;

        _settingsProvided = false;
        _client = null!;

        _valuesReceivedList = new List<double>(Constants.TotalRegisters);
    }

    public void SetComSettings(ModbusComSettings portSettings)
    {
        lock (this)
        {
            if (_isRunning || _client != null)
            {
                throw new InvalidOperationException(
                    $"MODBUS communication settings cannot be updated while running");
            }
            else
            {
                _settingsProvided = true;

                _comPort = portSettings.ComPort;

                _deviceAddress = portSettings.DeviceAddress;
                _registersStartAddress = Convert.ToInt32(portSettings.RegistersStartAddress, 16);

                _baudRate = portSettings.BaudRate;
                _dataBits = portSettings.DataBits;
                _parity = portSettings.Parity;
                _stopBits = portSettings.StopBits;
            }
        }
    }

    #region Start / Stop Acquisition
    public void StartAcquisition()
    {
        lock (this)
        {
            if (_isRunning || _client != null)
            {
                throw new InvalidOperationException(
                    $"{nameof(StartAcquisition)} cannot be invoked while operation" +
                    $" is in progress.");
            }
            else
            {
                _logger.LogInformation(
                    $"Starting data acquisition from {_comPort}" +
                    $" @{Constants.DataAcquisitionIntervalMS}ms interval");

                CreateComClientObject();

                _client?.Connect(_comPort, Constants.ModbusEndianness);

                _isRunning = true;

                _timer.Change(
                    TimeSpan.FromMilliseconds(Constants.DataAcquisitionIntervalMS),
                    TimeSpan.FromMilliseconds(Constants.DataAcquisitionIntervalMS));
            }
        }
    }

    public void StopAcquisition()
    {
        if (_isRunning)
        {
            _logger.LogInformation($"Stopping data acquisition from {_comPort}");

            _isRunning = false;
        }
    }

    private void CreateComClientObject()
    {
        if (_settingsProvided == false)
        {
            throw new InvalidOperationException(
                "MODBUS cannot be initialized without providing settings");
        }
        else
        {
            System.IO.Ports.Parity parity = System.IO.Ports.Parity.None;
            System.IO.Ports.StopBits stopBits = System.IO.Ports.StopBits.None;

            switch (_parity)
            {
                case Entities.Parity.None:
                    parity = System.IO.Ports.Parity.None;
                    break;

                case Entities.Parity.Odd:
                    parity = System.IO.Ports.Parity.Odd;
                    break;

                case Entities.Parity.Even:
                    parity = System.IO.Ports.Parity.Even;
                    break;
            }

            switch (_stopBits)
            {
                case Entities.StopBits.One:
                    stopBits = System.IO.Ports.StopBits.One;
                    break;

                case Entities.StopBits.Two:
                    stopBits = System.IO.Ports.StopBits.Two;
                    break;

                case Entities.StopBits.OnePointFive:
                    stopBits = System.IO.Ports.StopBits.OnePointFive;
                    break;
            }

            _client = new ModbusRtuClient()
            {
                BaudRate = (int)_baudRate,
                Parity = parity,
                StopBits = stopBits
            };
        }
    }
    #endregion

    #region Acquire Data
    private void AcquireData(object? obj)
    {
        _timer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

        short[] shortData = null!;

        try
        {
            shortData = _client.ReadHoldingRegisters<short>(
                _deviceAddress,
                _registersStartAddress,
                Constants.TotalRegisters).ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Reading error at {_comPort}", ex.Message);
        }

        if (shortData != null &&
            shortData.Length <= Constants.TotalRegisters &&
            shortData.Length >= 1)
        {
            for (int i = 0; i < shortData.Length; i++)
            {
                _valuesReceivedList[i] = ((double)shortData[i]) / 1000.0;
            }

            NewValuesReceived?.Invoke(_valuesReceivedList);
        }

        if (_isRunning)
        {
            _timer.Change(
                TimeSpan.FromMilliseconds(Constants.DataAcquisitionIntervalMS),
                TimeSpan.FromMilliseconds(Constants.DataAcquisitionIntervalMS));
        }
        else
        {
            _client.Dispose();
            _client = null!;

            _logger.LogInformation($"Stopped data acquisition from {_comPort}");
        }
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
    #endregion
}
