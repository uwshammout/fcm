using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace CronBlocks.UserControls.Wpf.ThreePointerDial;

internal partial class ThreePointerDialViewModel : ObservableObject
{
    public ThreePointerDialViewModel()
    {

    }

    [ObservableProperty]
    private bool dial1Visible = true;
    [ObservableProperty]
    private bool dial2Visible = true;
    [ObservableProperty]
    private bool dial3Visible = true;

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
    private double dial1Rotation = 0.0;
    [ObservableProperty]
    private double dial2Rotation = 0.0;
    [ObservableProperty]
    private double dial3Rotation = 0.0;
}
