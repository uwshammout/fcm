using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace CronBlocks.UserControls.Wpf.ThreePointerDial;

internal partial class ThreePointerDialViewModel : ObservableObject
{
    public ThreePointerDialViewModel()
    {
        MinRotation = -60;
        MaxRotation = +60;
    }

    [ObservableProperty]
    private Visibility dial1Visibility = Visibility.Visible;
    [ObservableProperty]
    private Visibility dial2Visibility = Visibility.Visible;
    [ObservableProperty]
    private Visibility dial3Visibility = Visibility.Visible;

    [ObservableProperty]
    private Brush backgroundColor = new SolidColorBrush(Color.FromArgb(255, 114, 114, 114));
    [ObservableProperty]
    private Brush borderColor = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));

    [ObservableProperty]
    private Brush dial1Color = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
    [ObservableProperty]
    private Brush dial2Color = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
    [ObservableProperty]
    private Brush dial3Color = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

    [ObservableProperty]
    private Brush dial1BorderColor = new SolidColorBrush(Color.FromArgb(255, 100, 0, 0));
    [ObservableProperty]
    private Brush dial2BorderColor = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
    [ObservableProperty]
    private Brush dial3BorderColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 100));

    [ObservableProperty]
    private double minRotation;
    [ObservableProperty]
    private double maxRotation;

    [ObservableProperty]
    private double dial1Rotation = 0.0;
    [ObservableProperty]
    private double dial2Rotation = 0.0;
    [ObservableProperty]
    private double dial3Rotation = 0.0;
}
