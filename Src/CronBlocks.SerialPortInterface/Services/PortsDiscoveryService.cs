using CronBlocks.SerialPortInterface.Configuration;
using CronBlocks.SerialPortInterface.Interfaces;
using System.IO.Ports;

namespace CronBlocks.SerialPortInterface.Services;

public class PortsDiscoveryService : IPortsDiscoveryService
{
    private bool _isRunning = false;
    private Timer _timer;
    private List<string> _foundPortsList;

    public event Action<string>? NewPortFound;
    public event Action<string>? ExistingPortRemoved;

    public PortsDiscoveryService()
    {
        _foundPortsList = new List<string>();

        _isRunning = false;
        _timer = new Timer(
            UpdatePorts, null,
            TimeSpan.FromMilliseconds(-1),
            TimeSpan.FromMilliseconds(-1));
    }

    #region Starting and Stopping the Ports' Discovery
    public void StartPortsDiscovery()
    {
        _foundPortsList.Clear();

        _isRunning = true;
        _timer.Change(
            TimeSpan.FromMilliseconds(Constants.PortDiscoveryIntervalMS),
            TimeSpan.FromMilliseconds(Constants.PortDiscoveryIntervalMS));
    }

    public void StopPortsDiscovery()
    {
        _isRunning = false;
        _timer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));
    }
    #endregion

    #region Ports Discovery
    private void UpdatePorts(object? _)
    {
        _timer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

        lock (this)
        {
            List<string> systemPorts = new List<string>(SerialPort.GetPortNames());
            List<string> portsToBeRemoved = new List<string>();

            // Finding newly added ports
            foreach (string port in systemPorts)
            {
                if (!_foundPortsList.Contains(port))
                {
                    _foundPortsList.Add(port);
                    NewPortFound?.Invoke(port);
                }
            }

            // Finding ports that no longer exist
            foreach (string port in _foundPortsList)
            {
                if (!systemPorts.Contains(port))
                {
                    portsToBeRemoved.Add(port);
                    ExistingPortRemoved?.Invoke(port);
                }
            }

            foreach (string port in portsToBeRemoved)
            {
                _foundPortsList.Remove(port);
            }
        }

        if (_isRunning)
        {
            _timer.Change(
                TimeSpan.FromMilliseconds(Constants.PortDiscoveryIntervalMS),
                TimeSpan.FromMilliseconds(Constants.PortDiscoveryIntervalMS));
        }
    }
    #endregion
}
