namespace CronBlocks.SerialPortInterface.Interfaces
{
    public interface IModbusAcquisitionService : IDisposable
    {
        event Action<List<double>>? NewValuesReceived;

        void StartAcquisition();
        void StopAcquisition();
    }
}