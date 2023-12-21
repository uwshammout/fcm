using System.Windows.Controls;

namespace CronBlocks.UserControls.Wpf.ThreePointerDial;

public partial class ThreePointerDial : UserControl
{
    public ThreePointerDial()
    {
        InitializeComponent();

        DataContext = new ThreePointerDialViewModel();
    }
}
