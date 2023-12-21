using CronBlocks.SerialPortInterface.Entities;

namespace CronBlocks.SerialPortInterface.Interfaces;

public interface ISerialModbusClientService : IDisposable
{
    event Action<List<double>>? NewValuesReceived;

    void SetComSettings(SerialModbusClientSettings portSettings);
    void StartAcquisition();
    void StopAcquisition();
}