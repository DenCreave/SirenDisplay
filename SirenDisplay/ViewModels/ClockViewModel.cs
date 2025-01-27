using System;
using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using ExCSS;
using SirenDisplay.Assets.Polygons.Buttons;
using SirenDisplay.Assets.Polygons.Frames;
using SirenDisplay.Classes.Digits;
using Color = Avalonia.Media.Color;
using FontStretch = Avalonia.Media.FontStretch;
using FontStyle = Avalonia.Media.FontStyle;
using FontWeight = Avalonia.Media.FontWeight;
using HorizontalAlignment = Avalonia.Layout.HorizontalAlignment;
using VerticalAlignment = Avalonia.Layout.VerticalAlignment;

namespace SirenDisplay.ViewModels;

public sealed partial class ClockViewModel : ObservableObject
{
    
    [ObservableProperty] private Path _mypathFigures;
    [ObservableProperty] private Path _hourDecimalDigit;
    [ObservableProperty] private Path _minuteDecimalDigit;
    [ObservableProperty] private Path _hourDigit;
    [ObservableProperty] private Path _minuteDigit;
    private DigitLoader _digitLoader;
    private DispatcherTimer _timer;
    [ObservableProperty] private Button _alarmButton;
    
    public ClockViewModel()
    {
        FrameInitializer();
        ClockInitializer();
        ButtonInitializer();

        Console.WriteLine("ClockViewModel constructor complete");
    }

    private void FrameInitializer()
    {
        MypathFigures = new Path
        {

            Stroke =  new SolidColorBrush(Color.FromArgb(255, 0xff, 0x90, 0x1b)),
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
            Effect = new DropShadowEffect
            {
                Color = Color.FromArgb(255, 0xff, 0x90, 0x1b),
                Opacity = 1,
                BlurRadius = 500
            },
        };
    }

    private void ClockInitializer()
    {
        Path GeneratePathDefaults()
        {
            return new Path
            {
                //todo, generate a class for managing style and effects to make it responsive
                Stroke =  new SolidColorBrush(Color.FromArgb(255, 0xff, 0x90, 0x1b)),
                StrokeThickness = 10,
                //Margin = new Thickness(),
                Stretch = Stretch.Uniform,
                Data = _digitLoader.ReturnPathGeometry(10),
                /*Effect = new DropShadowEffect
                {
                    Color = Color.FromArgb(255, 0xff, 0x90, 0x1b),
                    Opacity = 1,
                    BlurRadius = 500
                }*/
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
            int hours = DateTime.Now.Hour;
            int hoursDecimal = hours / 10;
            int minutes = DateTime.Now.Minute;
            int minutesDecimal = minutes / 10;
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
    
    private void ButtonInitializer()
    {
        PathGeometry pathGeometry = new PathGeometry()
        {
            Figures = new ActivateAlarm().Rectangles
        };
        Button tryme= new Button
        {
           // Transitions = null,
           // Name = null,
           // DataContext = null,
           // Resources = null,
           // Theme = null,
            Opacity = 1,
           // OpacityMask = null,
          //  Effect = null,
            Focusable = false,
           // IsEnabled = false,
            Background = new DrawingBrush
            {
                Drawing = new DrawingGroup()pathGeometry
            },
            BackgroundSizing = BackgroundSizing.InnerBorderEdge,
           // BorderBrush = null,
           // BorderThickness = default,
           // CornerRadius = default,
           // FontFamily = null,
           // FontFeatures = null,
            FontSize = 0,
            FontStyle = FontStyle.Normal,
            FontWeight = (FontWeight)0,
            FontStretch = (FontStretch)0,
           // Foreground = null,
           
        };

    }
}