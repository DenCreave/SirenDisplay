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
    public CacheVM CacheVM { get; set; }
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
    }
    
    
    
    
    
}