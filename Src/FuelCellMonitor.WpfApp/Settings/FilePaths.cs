using System.IO;

namespace CronBlocks.FuelCellMonitor.Settings;

internal static class FilePaths
{
    public static readonly string FilesDirectory =
        $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}" +
        $"{Path.DirectorySeparatorChar}CronBlocks" +
        $"{Path.DirectorySeparatorChar}FuelCellMonitor" +
        $"{Path.DirectorySeparatorChar}";

    public static readonly string DataScalingFilename = $"{FilesDirectory}ScalingFactors.ini";
}
