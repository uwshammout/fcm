using CronBlocks.SerialPortInterface.Entities;
using CronBlocks.SerialPortInterface.Interfaces;

namespace CronBlocks.SerialPortInterface.Services;

public class SerialOptionsService : ISerialOptionsService
{
    public IEnumerable<string> GetAllOptions<T>()
    {
        if (typeof(T).IsEnum)
        {
            if (typeof(T) == typeof(BaudRate))
            {
                foreach (BaudRate baudRate in Enum.GetValues(typeof(BaudRate)))
                {
                    yield return baudRate.ToString().Replace("_", "");
                }
            }
            else if (typeof(T) == typeof(DataBits))
            {
                foreach (DataBits dataBits in Enum.GetValues(typeof(DataBits)))
                {
                    yield return dataBits.ToString().Replace("_", "");
                }
            }
            else if (typeof(T) == typeof(Parity))
            {
                foreach (Parity parity in Enum.GetValues(typeof(Parity)))
                {
                    yield return parity.ToString();
                }
            }
            else if (typeof(T) == typeof(StopBits))
            {
                foreach (StopBits stopBits in Enum.GetValues(typeof(StopBits)))
                {
                    switch (stopBits)
                    {
                        case StopBits.One: yield return "1"; break;
                        case StopBits.Two: yield return "2"; break;
                        case StopBits.OnePointFive: yield return "1.5"; break;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"Invalid use of {nameof(GetAllOptions)} for Enum type {typeof(T)}");
            }
        }
        else
        {
            throw new InvalidOperationException(
                $"{nameof(GetAllOptions)} is only valid for Enum types");
        }
    }

    public T ConvertOption<T>(string option)
    {
        if (typeof(T).IsEnum &&
            !string.IsNullOrEmpty(option))
        {
            if (typeof(T) == typeof(BaudRate))
            {
                string enumStr = $"_{option}";

                if (Enum.IsDefined(typeof(BaudRate), enumStr))
                {
                    if (Enum.TryParse(typeof(BaudRate), enumStr, out object? baudRate))
                    {
                        return (T)baudRate!;
                    }
                }
            }
            else if (typeof(T) == typeof(DataBits))
            {
                string enumStr = $"_{option}";

                if (Enum.IsDefined(typeof(DataBits), enumStr))
                {
                    if (Enum.TryParse(typeof(DataBits), enumStr, out object? dataBits))
                    {
                        return (T)dataBits!;
                    }
                }
            }
            else if (typeof(T) == typeof(Parity))
            {
                string enumStr = $"{option}";

                if (Enum.IsDefined(typeof(Parity), enumStr))
                {
                    if (Enum.TryParse(typeof(Parity), enumStr, out object? parity))
                    {
                        return (T)parity!;
                    }
                }
            }
            else if (typeof(T) == typeof(StopBits))
            {
                return (option switch
                {
                    "1" => (T)(object)StopBits.One,
                    "2" => (T)(object)StopBits.Two,
                    "1.5" => (T)(object)StopBits.OnePointFive,
                    _ => throw new InvalidOperationException(
                        $"{nameof(ConvertOption)} cannot convert '{option}' to {typeof(T)}")
                });
            }
            else
            {
                throw new InvalidOperationException(
                    $"Invalid use of {nameof(ConvertOption)} for Enum type {typeof(T)}");
            }
        }

        throw new InvalidOperationException(
                $"{nameof(ConvertOption)} cannot convert '{option}' to {typeof(T)}");
    }
}
