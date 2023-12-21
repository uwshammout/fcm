namespace CronBlocks.SerialPortInterface.Interfaces;

public interface ISerialPortsDiscoveryService
{
    event Action<string>? NewPortFound;
    event Action<string>? ExistingPortRemoved;

    void StartPortsDiscovery();
    void StopPortsDiscovery();
}