using CronBlocks.SerialPortInterface.Entities;

namespace CronBlocks.SerialPortInterface.Interfaces;

public interface IModbusAcquisitionService : IDisposable
{
    event Action<List<double>>? NewValuesReceived;

    void SetComSettings(ModbusComSettings portSettings);
    void StartAcquisition();
    void StopAcquisition();
}