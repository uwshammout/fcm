using CronBlocks.SerialPortInterface.Configuration;
using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Interfaces;
using FluentModbus;
using Microsoft.Extensions.Logging;

namespace CronBlocks.SerialPortInterface.Services;

public class ModbusAcquisitionService : IModbusAcquisitionService
{
    public event Action<List<double>>? NewValuesReceived;

    private bool _isRunning = false;
    private Timer _timer;

    private readonly ILogger<ModbusAcquisitionService> _logger;

    private ModbusComSettings _portSettings;

    private ModbusRtuClient _client;
    private int _deviceAddress;
    private int _startAddress;
    private List<double> _valuesReceivedList;

    public ModbusAcquisitionService(
        ModbusComSettings portSettings,
        ILogger<ModbusAcquisitionService> logger)
    {
        _isRunning = false;
        _timer = new Timer(AcquireData, null, TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

        this._portSettings = portSettings;
        this._logger = logger;

        System.IO.Ports.Parity parity = System.IO.Ports.Parity.None;
        System.IO.Ports.StopBits stopBits = System.IO.Ports.StopBits.None;

        switch (portSettings.Parity)
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

        switch (portSettings.StopBits)
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
            BaudRate = (int)portSettings.BaudRate,
            Parity = parity,
            StopBits = stopBits
        };

        _deviceAddress = portSettings.DeviceAddress;
        _startAddress = Convert.ToInt32(portSettings.RegistersStartAddress, 16);

        _valuesReceivedList = new List<double>(Constants.TotalRegisters);
    }

    #region Start / Stop Acquisition
    public void StartAcquisition()
    {
        _logger.LogInformation(
            $"Starting data acquisition from {_portSettings.ComPort}" +
            $" @{Constants.DataAcquisitionIntervalMS}ms interval");

        _client.Connect(_portSettings.ComPort, Constants.ModbusEndianness);

        _isRunning = true;
        _timer.Change(
            TimeSpan.FromMilliseconds(Constants.DataAcquisitionIntervalMS),
            TimeSpan.FromMilliseconds(Constants.DataAcquisitionIntervalMS));
    }

    public void StopAcquisition()
    {
        _logger.LogInformation($"Stopping data acquisition from {_portSettings.ComPort}");

        _isRunning = false;
        _timer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));
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
                _startAddress,
                Constants.TotalRegisters).ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Reading error at {_portSettings.ComPort}", ex.Message);
        }

        if (shortData != null &&
            shortData.Length <= Constants.TotalRegisters &&
            shortData.Length >= 1)
        {
            ModbusComSettings ps;

            lock (this)
            {
                ps = _portSettings;
            }

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
    }

    public void Dispose()
    {
        _client.Dispose();
    }
    #endregion
}
