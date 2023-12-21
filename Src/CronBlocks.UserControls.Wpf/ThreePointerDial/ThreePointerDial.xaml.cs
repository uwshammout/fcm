using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CronBlocks.UserControls.Wpf.ThreePointerDial;

public partial class ThreePointerDial : UserControl
{
    private ThreePointerDialViewModel viewModel;

    public ThreePointerDial()
    {
        InitializeComponent();

        viewModel = new ThreePointerDialViewModel();
        DataContext = viewModel;

        dial1Visible = viewModel.Dial1Visible;
        dial2Visible = viewModel.Dial2Visible;
        dial3Visible = viewModel.Dial3Visible;

        backgroundColor = viewModel.BackgroundColor;
        borderColor = viewModel.BorderColor;

        dial1Color = viewModel.Dial1Color;
        dial2Color = viewModel.Dial2Color;
        dial3Color = viewModel.Dial3Color;

        dial1BorderColor = viewModel.Dial1BorderColor;
        dial2BorderColor = viewModel.Dial2BorderColor;
        dial3BorderColor = viewModel.Dial3BorderColor;
    }

    #region Dial Visibility
    private Visibility dial1Visible;
    private Visibility dial2Visible;
    private Visibility dial3Visible;

    public Visibility Dial1Visible
    {
        get => dial1Visible;
        set
        {
            if (dial1Visible != value)
            {
                dial1Visible = value;
                viewModel.Dial1Visible = value;
            }
        }
    }

    public Visibility Dial2Visible
    {
        get => dial2Visible;
        set
        {
            if (dial2Visible != value)
            {
                dial2Visible = value;
                viewModel.Dial2Visible = value;
            }
        }
    }

    public Visibility Dial3Visible
    {
        get => dial3Visible;
        set
        {
            if (dial3Visible != value)
            {
                dial3Visible = value;
                viewModel.Dial3Visible = value;
            }
        }
    }
    #endregion
    #region Colors
    private Brush backgroundColor;
    private Brush borderColor;

    public Brush BackgroundColor
    {
        get => backgroundColor;
        set
        {
            if (backgroundColor != value)
            {
                backgroundColor = value;
                viewModel.BackgroundColor = value;
            }
        }
    }

    public Brush BorderColor
    {
        get => borderColor;
        set
        {
            if (borderColor != value)
            {
                borderColor = value;
                viewModel.BorderColor = value;
            }
        }
    }

    private Brush dial1Color;
    private Brush dial2Color;
    private Brush dial3Color;

    public Brush Dial1Color
    {
        get => dial1Color;
        set
        {
            if (dial1Color != value)
            {
                dial1Color = value;
                viewModel.Dial1Color = value;
            }
        }
    }

    public Brush Dial2Color
    {
        get => dial2Color;
        set
        {
            if (dial2Color != value)
            {
                dial2Color = value;
                viewModel.Dial2Color = value;
            }
        }
    }

    public Brush Dial3Color
    {
        get => dial3Color;
        set
        {
            if (dial3Color != value)
            {
                dial3Color = value;
                viewModel.Dial3Color = value;
            }
        }
    }

    private Brush dial1BorderColor;
    private Brush dial2BorderColor;
    private Brush dial3BorderColor;

    public Brush Dial1BorderColor
    {
        get => dial1BorderColor;
        set
        {
            if (dial1BorderColor != value)
            {
                dial1BorderColor = value;
                viewModel.Dial1BorderColor = value;
            }
        }
    }

    public Brush Dial2BorderColor
    {
        get => dial2BorderColor;
        set
        {
            if (dial2BorderColor != value)
            {
                dial2BorderColor = value;
                viewModel.Dial2BorderColor = value;
            }
        }
    }

    public Brush Dial3BorderColor
    {
        get => dial3BorderColor;
        set
        {
            if (dial3BorderColor != value)
            {
                dial3BorderColor = value;
                viewModel.Dial3BorderColor = value;
            }
        }
    }
    #endregion
    #region Dial Values
    private double gaugeMinValue = 0.0;
    private double gaugeMaxValue = 10.0;

    private double dial1Value = 0.0;
    private double dial2Value = 0.0;
    private double dial3Value = 0.0;
    #endregion
}
