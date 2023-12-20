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

    public ModbusAcquisitionService(ILogger<ModbusAcquisitionService> logger)
    {
        _isRunning = false;
        _timer = new Timer(AcquireData, null, TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

        _logger = logger;

        _portSettings = null!;
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
                _portSettings = portSettings;
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
                    $"Starting data acquisition from {_portSettings.ComPort}" +
                    $" @{Constants.DataAcquisitionIntervalMS}ms interval");

                InitializeComObject();

                _client?.Connect(_portSettings.ComPort, Constants.ModbusEndianness);

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
            _logger.LogInformation($"Stopping data acquisition from {_portSettings.ComPort}");

            _isRunning = false;
        }
    }

    private void InitializeComObject()
    {
        if (_portSettings == null)
        {
            throw new InvalidOperationException(
                "MODBUS cannot be initialized without providing settings");
        }
        else
        {
            System.IO.Ports.Parity parity = System.IO.Ports.Parity.None;
            System.IO.Ports.StopBits stopBits = System.IO.Ports.StopBits.None;

            switch (_portSettings.Parity)
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

            switch (_portSettings.StopBits)
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
                BaudRate = (int)_portSettings.BaudRate,
                Parity = parity,
                StopBits = stopBits
            };

            _deviceAddress = _portSettings.DeviceAddress;
            _startAddress = Convert.ToInt32(_portSettings.RegistersStartAddress, 16);
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

            _logger.LogInformation($"Stopped data acquisition from {_portSettings.ComPort}");
        }
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
    #endregion
}
