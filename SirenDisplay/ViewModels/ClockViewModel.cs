using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Assets.Polygons.Frames;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Classes.Digits;
using SirenDisplay.Controllers;
using SirenDisplay.Model;
using SirenDisplay.Services;
using Path = Avalonia.Controls.Shapes.Path;

namespace SirenDisplay.ViewModels;

public sealed partial class ClockViewModel : ViewModelBase
{
    [ObservableProperty] private Path _mainFrame;
    [ObservableProperty] private Path _hourDecimalDigit;
    [ObservableProperty] private Path _minuteDecimalDigit;
    [ObservableProperty] private Path _hourDigit;
    [ObservableProperty] private Path _minuteDigit;
    private DigitLoader _digitLoader;
    private DispatcherTimer _timer;
    public LabelData LabelData { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor( nameof(EnabledMe))]
    private bool _isGoodMorning;

    public bool EnabledMe => !IsGoodMorning;

    [ObservableProperty]
    private string _alarmString;


    [ObservableProperty] private string _selectedPlaylist;

    public AlarmTimerController AlarmTimer { get; } // Public so XAML can bind to it!
    public NavigationService Navigator { get; }
    private readonly SpanningTreeController _stc;


    public ClockViewModel(
        AlarmTimerController alarmTimer,
        NavigationService navigator,
        SpanningTreeController stc) 
    {
        Console.WriteLine("ClockViewModel was instantiated.");
        AlarmTimer = alarmTimer;
        Navigator = navigator;
        _stc = stc;


        FrameInitializer();
        ClockInitializer();
        AlarmButtonInitializer();
        
        LoadAlarmData();

    }

    public void RestartSTC()
    {
        _stc.RequestRestart();
    }
    
    

    private void FrameInitializer()
    {
        MainFrame = new Path
        {
            Stroke =  Application.Current.FindResource("OffColor") as LinearGradientBrush,
            StrokeThickness = 10,
            Stretch = Stretch.Fill,
            Data = new PathGeometry
            {
                Figures = new PathFigures
                {
                    new TopFrame().PathFigure,
                    new MiddleFrame().PathFigure,
                    new BottomFrame().PathFigure
                }
            },
            //Effect = Application.Current.FindResource("OffEffect") as DropShadowEffect
        };
    }
    
    private void ClockInitializer()
    {
        Path GeneratePathDefaults()
        {
            return new Path
            {
                //todo, generate a class for managing style and effects to make it responsive
                Stroke =  Application.Current.FindResource("OffColor") as LinearGradientBrush,
                Fill = Application.Current.FindResource("SirenColor") as LinearGradientBrush,
                StrokeThickness = 10,
                Stretch = Stretch.Uniform,
                Data = _digitLoader.ReturnPathGeometry(10),
            };
        }

        
        var framerate = TimeSpan.FromSeconds(1); //1 fps. should try 1/60
        _timer = new DispatcherTimer
        {
            Interval = framerate
        };
        _digitLoader = new DigitLoader();
        int hours = DateTime.Now.Hour;
        int hoursDecimal = hours / 10;
        int minutes = DateTime.Now.Minute;
        int minutesDecimal = minutes / 10;

        HourDigit = GeneratePathDefaults();
        HourDecimalDigit = GeneratePathDefaults();
        MinuteDigit = GeneratePathDefaults();
        MinuteDecimalDigit = GeneratePathDefaults();
        
        _timer.Tick += (sender, args) =>
        {
            hours = DateTime.Now.Hour;
            hoursDecimal = hours / 10;
            minutes = DateTime.Now.Minute;
            minutesDecimal = minutes / 10;
            if (hoursDecimal == 0)
            {
                HourDecimalDigit.Data=_digitLoader.ReturnPathGeometry(10); 
            }
            else
            { 
                HourDecimalDigit.Data=_digitLoader.ReturnPathGeometry(hoursDecimal%3);
            }
            HourDigit.Data=_digitLoader.ReturnPathGeometry(hours%10);
            MinuteDecimalDigit.Data=_digitLoader.ReturnPathGeometry(minutesDecimal%6);
            MinuteDigit.Data=_digitLoader.ReturnPathGeometry(minutes%10);
        };
        _timer.Start();
        hours = DateTime.Now.Hour;
        hoursDecimal = hours / 10;
        minutes = DateTime.Now.Minute;
        minutesDecimal = minutes / 10;
        if (hoursDecimal == 0)
        {
            HourDecimalDigit.Data=_digitLoader.ReturnPathGeometry(10); 
        }
        else
        { 
            HourDecimalDigit.Data=_digitLoader.ReturnPathGeometry(hoursDecimal%3);
        }
        HourDigit.Data=_digitLoader.ReturnPathGeometry(hours%10);
        MinuteDecimalDigit.Data=_digitLoader.ReturnPathGeometry(minutesDecimal%6);
        MinuteDigit.Data=_digitLoader.ReturnPathGeometry(minutes%10);
    }
    
    private void AlarmButtonInitializer()
    {
        LabelData = new LabelData();
    }
    public void ActivateAlarmButton()
    {
        AlarmTimer.ActivateAlarmTimer();
    }

    private void LoadAlarmData()
    {
        var tmp=AlarmTimer.SirenData;
        AlarmString = $"{tmp.UsualTime.Hours}:{(tmp.UsualTime.Minutes>9?tmp.UsualTime.Minutes:(tmp.UsualTime.Minutes==0?"00":"0"+tmp.UsualTime.Minutes))}";
        SelectedPlaylist = string.IsNullOrEmpty(tmp.SelectedPlaylist) ? "Welcome to Siren Display" : tmp.SelectedPlaylist;
    }
    
}