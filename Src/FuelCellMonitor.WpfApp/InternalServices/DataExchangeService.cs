using CronBlocks.FuelCellMonitor.Settings;
using CronBlocks.Helpers;

namespace CronBlocks.FuelCellMonitor.InternalServices;

public class DataExchangeService : IDisposable
{
    private readonly IniConfigIO _iniConfig;

    public double ExperimentTimeDuration { get; set; }

    public DataExchangeService(IniConfigIO iniConfig)
    {
        _iniConfig = iniConfig;

        LoadConfig();
    }

    private void LoadConfig()
    {
        ExperimentTimeDuration = _iniConfig.GetDouble(KeyOf(nameof(ExperimentTimeDuration)), ValueConstants.ExperimentTimeMinimumSec + 20);
    }

    private void SaveConfig()
    {
        _iniConfig.SetDouble(KeyOf(nameof(ExperimentTimeDuration)), ExperimentTimeDuration);

        _iniConfig.SaveFile();
    }

    private string KeyOf(string paramName)
    {
        const string sectionName = nameof(DataExchangeService);
        return $"{sectionName}/{paramName}";
    }

    public void Dispose()
    {
        SaveConfig();
    }
}
