namespace CronBlocks.SerialPortInterface.Interfaces;

public interface ISerialOptionsService
{
    IEnumerable<string> GetAllOptions<T>();
    T ConvertOption<T>(string option);
}