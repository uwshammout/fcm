namespace CronBlocks.SerialPortInterface.Interfaces;

public interface IPortsDiscoveryService
{
    event Action<string>? NewPortFound;
    event Action<string>? ExistingPortRemoved;

    void StartPortsDiscovery();
    void StopPortsDiscovery();
}