using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Assets.Polygons.Frames;
using SirenDisplay.Classes.Digits;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.ViewModels;

public sealed partial class AlarmViewModel : ViewModelBase, IAlarmTimeController
{
    [ObservableProperty] private Path _mainFrame;
    [ObservableProperty] private Path _hourDecimalDigit;
    [ObservableProperty] private Path _minuteDecimalDigit;
    [ObservableProperty] private Path _hourDigit;
    [ObservableProperty] private Path _minuteDigit;
    private DigitLoader _digitLoader;
    public CacheReferences CacheReferences { get; set; }
    public LabelData LabelData { get; }


    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(ClockButton))]
    private AlarmState _alarmState;

    private LabelData _labelData;

    public string ClockButton
    {
        get {
            switch (AlarmState)
            {
                case AlarmState.Off:
                {
                    return _labelData.OffLabel;
                }
                case AlarmState.Pending:
                {
                    return _labelData.PendingLabel;
                }
                case AlarmState.Sirens:
                {
                    return _labelData.SirenLabel;
                }
                default:
                {
                    Console.WriteLine("Unknown alarm state");
                    return "\uE4E0";
                }
            }
        }
    }

    public AlarmViewModel()
    {
        //todo constructor
        TimeSpan tmp = new TimeSpan(0, 0, 0);
        //todo loadconf alarm time and set iarthours
    }
    
    
    
    
    #region IAlarmTimeController
    public int IATCHours { get; set; }
    public int IATCMinutes { get; set; }

    public void IncreaseMinute()
    {
        throw new NotImplementedException();
    }

    public void DecreaseMinute()
    {
        throw new NotImplementedException();
    }

    public void IncreaseMinuteDecimal()
    {
        throw new NotImplementedException();
    }

    public void DecreaseMinuteDecimal()
    {
        throw new NotImplementedException();
    }

    public void IncreaseHour()
    {
        throw new NotImplementedException();
    }

    public void DecreaseHour()
    {
        throw new NotImplementedException();
    }

    public void IncreaseHourDecimal()
    {
        throw new NotImplementedException();
    }

    public void DecreaseHourDecimal()
    {
        throw new NotImplementedException();
    }
    #endregion IAlarmTimeController
}