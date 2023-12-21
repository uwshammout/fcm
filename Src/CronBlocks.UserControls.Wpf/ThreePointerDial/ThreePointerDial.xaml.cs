using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;

namespace CronBlocks.UserControls.Wpf.ThreePointerDial;

public partial class ThreePointerDial : UserControl, INotifyPropertyChanged
{
    public ThreePointerDial()
    {
        InitializeComponent();
    }

    #region Colors
    private Brush _backgroundColor = new SolidColorBrush(Color.FromArgb(255, 14, 14, 14));
    private Brush _borderColor = new SolidColorBrush(Color.FromArgb(255, 200, 80, 80));

    private Brush _gauge1Color = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
    private Brush _gauge2Color = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
    private Brush _gauge3Color = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

    private Brush _gauge1BorderColor = new SolidColorBrush(Color.FromArgb(255, 100, 0, 0));
    private Brush _gauge2BorderColor = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
    private Brush _gauge3BorderColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 100));

    public Brush BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            if (value != _backgroundColor)
            {
                _backgroundColor = value;
                NotifyPropertyChanged();
            }
        }
    }

    public Brush BorderColor
    {
        get => _borderColor;
        set
        {
            if (value != _borderColor)
            {
                _borderColor = value;
                NotifyPropertyChanged();
            }
        }
    }

    public Brush Gauge1Color
    {
        get => _gauge1Color;
        set
        {
            if (_gauge1Color != value)
            {
                _gauge1Color = value;
                NotifyPropertyChanged();
            }
        }
    }

    public Brush Gauge2Color
    {
        get => _gauge2Color;
        set
        {
            if (_gauge2Color != value)
            {
                _gauge2Color = value;
                NotifyPropertyChanged();
            }
        }
    }

    public Brush Gauge3Color
    {
        get => _gauge3Color;
        set
        {
            if (_gauge3Color != value)
            {
                _gauge3Color = value;
                NotifyPropertyChanged();
            }
        }
    }

    public Brush Gauge1BorderColor
    {
        get => _gauge1BorderColor;
        set
        {
            if (_gauge1BorderColor != value)
            {
                _gauge1BorderColor = value;
                NotifyPropertyChanged();
            }
        }
    }

    public Brush Gauge2BorderColor
    {
        get => _gauge2BorderColor;
        set
        {
            if (_gauge2BorderColor != value)
            {
                _gauge2BorderColor = value;
                NotifyPropertyChanged();
            }
        }
    }

    public Brush Gauge3BorderColor
    {
        get => _gauge3BorderColor;
        set
        {
            if (_gauge3BorderColor != value)
            {
                _gauge3BorderColor = value;
                NotifyPropertyChanged();
            }
        }
    }
    #endregion
    #region Dial Enable / Disable Control
    private bool _dial1Enabled = true;
    private bool _dial2Enabled = true;
    private bool _dial3Enabled = true;

    public bool Dial1Enabled
    {
        get => _dial1Enabled;
        set
        {
            if (_dial1Enabled != value)
            {
                _dial1Enabled = value;
                NotifyPropertyChanged();
            }
        }
    }

    public bool Dial2Enabled
    {
        get => _dial2Enabled;
        set
        {
            if (_dial2Enabled != value)
            {
                _dial2Enabled = value;
                NotifyPropertyChanged();
            }
        }
    }

    public bool Dial3Enabled
    {
        get => _dial3Enabled;
        set
        {
            if (_dial3Enabled != value)
            {
                _dial3Enabled = value;
                NotifyPropertyChanged();
            }
        }
    }
    #endregion
    #region Dial Values
    private double _dial1Value = 0.0;
    private double _dial2Value = 0.0;
    private double _dial3Value = 0.0;

    public double Dial1Value
    {
        get => _dial1Value;
        set
        {
            if (_dial1Value != value)
            {
                _dial1Value = value;
                NotifyPropertyChanged();
            }
        }
    }

    public double Dial2Value
    {
        get => _dial2Value;
        set
        {
            if (_dial2Value != value)
            {
                _dial2Value = value;
                NotifyPropertyChanged();
            }
        }
    }

    public double Dial3Value
    {
        get => _dial3Value;
        set
        {
            if (_dial3Value != value)
            {
                _dial3Value = value;
                NotifyPropertyChanged();
            }
        }
    }
    #endregion
    #region Gauge Min / Max Values
    private double _gaugeMinValue = 0.0;
    private double _gaugeMaxValue = 10.0;

    public double GaugeMinValue
    {
        get => _gaugeMinValue;
        set
        {
            if (_gaugeMinValue != value)
            {
                _gaugeMinValue = value;
                NotifyPropertyChanged();
            }
        }
    }

    public double GaugeMaxValue
    {
        get => _gaugeMaxValue;
        set
        {
            if (_gaugeMaxValue != value)
            {
                _gaugeMaxValue = value;
                NotifyPropertyChanged();
            }
        }
    }
    #endregion
    #region Property Change Notification
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (!string.IsNullOrEmpty(propertyName))
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    #endregion
}
