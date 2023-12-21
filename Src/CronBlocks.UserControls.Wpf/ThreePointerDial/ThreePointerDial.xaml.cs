using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace CronBlocks.UserControls.Wpf.ThreePointerDial;

public partial class ThreePointerDial : UserControl, INotifyPropertyChanged
{
    public ThreePointerDial()
    {
        InitializeComponent();
    }

    #region Dial Enable / Disable Control
    private bool _dial1Enabled = true;
    private bool _dial2Enabled = true;
    private bool _dial3Enabled = true;

    public bool Dial1Enabled
    {
        get { return _dial1Enabled; }
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
        get { return _dial2Enabled; }
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
        get { return _dial3Enabled; }
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
        get { return _dial1Value; }
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
        get { return _dial2Value; }
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
        get { return _dial3Value; }
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
        get { return _gaugeMinValue; }
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
        get { return _gaugeMaxValue; }
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
