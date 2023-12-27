using CronBlocks.SerialPortInterface.Configuration;
using CronBlocks.SerialPortInterface.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.IO;

namespace CronBlocks.SerialPortInterface.Services;

public class SerialModbusDataScalingService : ISerialModbusDataScalingService
{
    public event Action<List<double>>? NewValuesReceived;

    private readonly ILogger<SerialModbusDataScalingService> _logger;
    private readonly ISerialModbusClientService _service;
    private readonly string _filename;

    private List<double> _scaledValues;
    private List<double> _multiplicationFactors;
    private List<double> _offsets;

    public SerialModbusDataScalingService(
        ILogger<SerialModbusDataScalingService> logger,
        ISerialModbusClientService service,
        string filename = null!)
    {
        _logger = logger;
        _service = service;
        _filename = filename;

        _scaledValues = new List<double>(Constants.TotalRegisters);
        _multiplicationFactors = new List<double>(Constants.TotalRegisters);
        _offsets = new List<double>(Constants.TotalRegisters);

        for (int i = 0; i < Constants.TotalRegisters; i++)
        {
            _multiplicationFactors[i] = 1;
            _offsets[i] = 0;
        }

        if (string.IsNullOrEmpty(_filename))
        {
            _logger.LogInformation(
                $"No filename is provided, so we are using defaults.");
        }
        else if (File.Exists(_filename) == false)
        {
            _logger.LogWarning(
                $"File '{_filename}' doesn't exist, so we are using defaults.");
        }
        else
        {
            LoadValuesFromFile();
        }

        _service.NewValuesReceived += OnNewModbusValuesReceived;
    }

    public ImmutableList<double> GetMultiplicationFactors()
    {
        return _multiplicationFactors.ToImmutableList();
    }

    public ImmutableList<double> GetOffsets()
    {
        return _offsets.ToImmutableList();
    }

    public void SetMultiplicationFactor(int index, double value)
    {
        if (index < 0 || index >= Constants.TotalRegisters)
        {
            throw new ArgumentOutOfRangeException("index");
        }

        _multiplicationFactors[index] = value;
    }

    public void SetOffset(int index, double value)
    {
        if (index < 0 || index >= Constants.TotalRegisters)
        {
            throw new ArgumentOutOfRangeException("index");
        }

        _offsets[index] = value;
    }

    private void LoadValuesFromFile()
    {
        // TODO: To be implemented
    }

    private void SaveValuesToFile()
    {
        // TODO: To be implemented
    }

    public void Dispose()
    {
        _service.NewValuesReceived -= OnNewModbusValuesReceived;
        SaveValuesToFile();
    }

    private void OnNewModbusValuesReceived(List<double> values)
    {
        if (values == null)
        {
            _logger.LogError(
                $"The list of 'values' received from MODBUS service is null.");

            return;
        }
        else if (values.Count < Constants.TotalRegisters)
        {
            _logger.LogError(
                $"The list of 'values' received from MODBUS" +
                $" has {values.Count} values, whereas" +
                $" {Constants.TotalRegisters} values are expected.");

            return;
        }

        for (int i = 0; i < Constants.TotalRegisters; i++)
        {
            _scaledValues[i] =
                (values[i] * _multiplicationFactors[i]) + _offsets[i];
        }

        NewValuesReceived?.Invoke(_scaledValues);
    }
}
