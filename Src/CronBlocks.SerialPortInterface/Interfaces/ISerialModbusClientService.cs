using CronBlocks.SerialPortInterface.Entities;

namespace CronBlocks.SerialPortInterface.Interfaces;

public interface ISerialModbusClientService : IDisposable
{
    event Action<List<double>>? NewValuesReceived;
    event Action<OperationState>? OperationStateChanged;

    void SetComSettings(SerialModbusClientSettings portSettings);
    SerialModbusClientSettings GetComSettings();

    OperationState OperationState { get; }

    void StartAcquisition();
    void StopAcquisition();
}