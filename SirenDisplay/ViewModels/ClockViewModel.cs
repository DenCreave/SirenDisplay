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
using SirenDisplay.Assets.Polygons.Frames;
using SirenDisplay.Classes.Digits;
using Color = Avalonia.Media.Color;

namespace SirenDisplay.ViewModels;

public sealed partial class ClockViewModel : ObservableObject
{
    
    [ObservableProperty] private Path _mypathFigures;
    [ObservableProperty] private Path _hourDecimalDigit;
    [ObservableProperty] private Path _minuteDecimalDigit;
    [ObservableProperty] private Path _hourDigit;
    [ObservableProperty] private Path _minuteDigit;
    public ClockViewModel()
    {
        
        MypathFigures = new Path
        {

            Stroke =  new SolidColorBrush(Color.FromArgb(255, 0xff, 0x90, 0x1b)),
            StrokeThickness = 10,
            //Margin = new Thickness(5),
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
        Console.WriteLine("ClockViewModel and something else");
        UpdateTime();
    }
    
    private void UpdateTime()
    {
        var framerate = TimeSpan.FromSeconds(1);
        var timer = new DispatcherTimer
        {
            Interval = framerate
        };
        int i = 0;
        
        DigitLoader digitLoader = new DigitLoader();
        
        int hours = DateTime.Now.Hour;
        int hoursDecimal = hours / 10;
        int minutes = DateTime.Now.Minute;
        int minutesDecimal = minutes / 10;
        HourDigit= new Path
        {

            Stroke =  new SolidColorBrush(Color.FromArgb(255, 0xff, 0x90, 0x1b)),
            StrokeThickness = 10,
            //Margin = new Thickness(5),
            Stretch = Stretch.Fill,
            Data = digitLoader.ReturnPathGeometry(10),
            Effect = new DropShadowEffect
            {
                Color = Color.FromArgb(255, 0xff, 0x90, 0x1b),
                Opacity = 1,
                BlurRadius = 500
            }
        };
        
        HourDecimalDigit= new Path
        {

            Stroke =  new SolidColorBrush(Color.FromArgb(255, 0xff, 0x90, 0x1b)),
            StrokeThickness = 10,
            //Margin = new Thickness(5),
            Stretch = Stretch.Fill,
            Data = digitLoader.ReturnPathGeometry(10),
            Effect = new DropShadowEffect
            {
                Color = Color.FromArgb(255, 0xff, 0x90, 0x1b),
                Opacity = 10,
                BlurRadius = 500
            }
        };
        
        MinuteDecimalDigit = new Path
        {

            Stroke =  new SolidColorBrush(Color.FromArgb(255, 0xff, 0x90, 0x1b)),
            StrokeThickness = 10,
            //Margin = new Thickness(5),
            Stretch = Stretch.Fill,
            Data = digitLoader.ReturnPathGeometry(10),
            Effect = new DropShadowEffect
            {
                Color = Color.FromArgb(255, 0xff, 0x90, 0x1b),
                Opacity = 1,
                BlurRadius = 500
            }
        };
        
        MinuteDigit = new Path
        {

            Stroke =  new SolidColorBrush(Color.FromArgb(255, 0xff, 0x90, 0x1b)),
            StrokeThickness = 10,
            //Margin = new Thickness(5),
            Stretch = Stretch.UniformToFill,
            Data = digitLoader.ReturnPathGeometry(10),
            Effect = new DropShadowEffect
            {
                Color = Color.FromArgb(255, 0xff, 0x90, 0x1b),
                Opacity = 1,
                BlurRadius = 500
            }
        };
        
        timer.Tick += (sender, args) =>
        {
            int hours = DateTime.Now.Hour;
            int hoursDecimal = hours / 10;
            int minutes = DateTime.Now.Minute;
            int minutesDecimal = minutes / 10;
            if (hoursDecimal == 0)
            {
                HourDecimalDigit.Data=digitLoader.ReturnPathGeometry(10); 
            }
            else
            { 
                HourDecimalDigit.Data=digitLoader.ReturnPathGeometry(hoursDecimal%3);
            }
            HourDigit.Data=digitLoader.ReturnPathGeometry(hours%10);
            MinuteDecimalDigit.Data=digitLoader.ReturnPathGeometry(minutesDecimal%6);
            MinuteDigit.Data=digitLoader.ReturnPathGeometry(minutes%10);
            i++;
            Console.WriteLine(i);

        };
        timer.Start();
        
    }
}