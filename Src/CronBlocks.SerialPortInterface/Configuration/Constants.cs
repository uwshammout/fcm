using FluentModbus;

namespace CronBlocks.SerialPortInterface.Configuration;

internal class Constants
{
    // Time Constants
    public static readonly double PortDiscoveryIntervalMS = 100;
    public static readonly double DataAcquisitionIntervalMS = 250;

    // Endianness
    public static readonly ModbusEndianness ModbusEndianness = ModbusEndianness.BigEndian;
}
