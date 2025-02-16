using System;
using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Reactive;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Svg.Skia;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExCSS;
using ReactiveUI;
using SirenDisplay.Assets.Polygons.Buttons;
using SirenDisplay.Assets.Polygons.Frames;
using SirenDisplay.Classes.Digits;
using SirenDisplay.Controllers;
using SirenDisplay.Model;
using SkiaSharp;
using Splat;
using Svg.Skia;
using Color = Avalonia.Media.Color;
using FontStretch = Avalonia.Media.FontStretch;
using FontStyle = Avalonia.Media.FontStyle;
using FontWeight = Avalonia.Media.FontWeight;
using HorizontalAlignment = Avalonia.Layout.HorizontalAlignment;
using Path = Avalonia.Controls.Shapes.Path;
using VerticalAlignment = Avalonia.Layout.VerticalAlignment;

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
    //todo put this to a different controller class, maybe make it a singleton
    //[NotifyPropertyChangedFor( nameof(TimeImagenda))]
    [ObservableProperty]
    [NotifyPropertyChangedFor( nameof(EnabledMe))]
    private bool _isGoodMorning;
    //public SvgImage TimeImagenda => new SvgImag { Source = SvgSource.Load($"avares://SirenDisplay/Assets/Images/{(IsGoodMorning ? "alarmbuttonver1" : "clockbuttonclock")}.svg") };
    public bool EnabledMe => !IsGoodMorning;
    
    public CacheReferences CacheReferences { get; set; }
    
    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(ClockButton))]
    private AlarmState _alarmState;
    
    public string ClockButton
    {
        get {
            switch (AlarmState)
            {
                case AlarmState.Off:
                {
                    return LabelData.OffLabel;
                }
                case AlarmState.Pending:
                {
                    return LabelData.PendingLabel;
                }
                case AlarmState.Sirens:
                {
                    return LabelData.SirenLabel;
                }
                default:
                {
                    Console.WriteLine("Unknown alarm state");
                    return "\uE4E0";
                }
            }
        }
    }

    [ObservableProperty]
    private string _alarmString;

    public ClockViewModel() 
    { 
        AlarmState = AlarmState.Off;
        FrameInitializer();
        ClockInitializer();
        AlarmButtonInitializer();
        Console.WriteLine("ClockViewModel constructor complete");
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
            Effect = Application.Current.FindResource("OffEffect") as DropShadowEffect
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
    }
    
    private void AlarmButtonInitializer()
    {
        LabelData = new LabelData();
        
    }




    public void ActivateAlarmButton()
    {
        Console.WriteLine($"alarm state was: {AlarmState}");
        AlarmState++;
        Console.WriteLine($"alarm state is : {AlarmState}");

    }

    public void PostLoad()
    {
        var tmp=CacheReferences.alarmTimerController.SirenData;
        AlarmString = $"{tmp.UsualTime.Hours}:{tmp.UsualTime.Minutes}";
        //todo playl$ist name
    }
    
}