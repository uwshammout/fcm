using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace CronBlocks.UserControls.Wpf.ThreePointerDial;

internal partial class ThreePointerDialViewModel : ObservableObject
{
    public ThreePointerDialViewModel()
    {

    }

    [ObservableProperty]
    private bool dial1Enabled = true;
    [ObservableProperty]
    private bool dial2Enabled = true;
    [ObservableProperty]
    private bool dial3Enabled = true;

    [ObservableProperty]
    private Brush backgroundColor = new SolidColorBrush(Color.FromArgb(255, 114, 114, 114));
    [ObservableProperty]
    private Brush borderColor = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));

    [ObservableProperty]
    private Brush gauge1Color = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
    [ObservableProperty]
    private Brush gauge2Color = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
    [ObservableProperty]
    private Brush gauge3Color = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

    [ObservableProperty]
    private Brush gauge1BorderColor = new SolidColorBrush(Color.FromArgb(255, 100, 0, 0));
    [ObservableProperty]
    private Brush gauge2BorderColor = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
    [ObservableProperty]
    private Brush gauge3BorderColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 100));

    [ObservableProperty]
    private double dial1Value = 0.0;
    [ObservableProperty]
    private double dial2Value = 0.0;
    [ObservableProperty]
    private double dial3Value = 0.0;

    [ObservableProperty]
    private double gaugeMinValue = 0.0;
    [ObservableProperty]
    private double gaugeMaxValue = 10.0;
}
