using CronBlocks.SerialPortInterface.Entities;
using FluentModbus;

namespace CronBlocks.SerialPortInterface.Configuration;

internal class Constants
{
    // Time Constants
    public const double PortDiscoveryIntervalMS = 100;
    public const double DataAcquisitionIntervalMS = 500;

    // Device Constants
    public const ModbusEndianness ModbusEndianness = FluentModbus.ModbusEndianness.BigEndian;
    public const int TotalRegisters = 16;

    // Defaults
    public const int DefaultDeviceAddress = 1;
    public const string DefaultRegistersStartAddressHexStr = "0x0020";

    public const BaudRate DefaultBaudRate = BaudRate._115200;
    public const DataBits DefaultDataBits = DataBits._8;
    public const Parity DefaultParity = Parity.None;
    public const StopBits DefaultStopBits = StopBits.One;
}
