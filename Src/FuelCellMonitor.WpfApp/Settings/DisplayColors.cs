using System.Windows.Media;

namespace CronBlocks.FuelCellMonitor.Settings;

internal static class DisplayColors
{
    public readonly static Brush ErrorBg = new SolidColorBrush(Color.FromArgb(200, 255, 174, 201));
    public readonly static Brush DisconnectButtonBg = new SolidColorBrush(Color.FromArgb(200, 218, 124, 231));
}
