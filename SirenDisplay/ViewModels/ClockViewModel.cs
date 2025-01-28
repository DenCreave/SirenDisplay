using System;
using System.ComponentModel;
using System.IO;
using System.Reactive;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
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
using SkiaSharp;
using Svg.Skia;
using Color = Avalonia.Media.Color;
using FontStretch = Avalonia.Media.FontStretch;
using FontStyle = Avalonia.Media.FontStyle;
using FontWeight = Avalonia.Media.FontWeight;
using HorizontalAlignment = Avalonia.Layout.HorizontalAlignment;
using Path = Avalonia.Controls.Shapes.Path;
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
    //todo put this to a different controller class, maybe make it a singleton
    
    [ObservableProperty]
    [NotifyPropertyChangedFor( nameof(TimeImagenda))]
    private bool _isGoodMorning;

    public SvgImage TimeImagenda => new SvgImage
        { Source = SvgSource.Load($"avares://SirenDisplay/Assets/Images/{(IsGoodMorning ? "alarmbuttonver1" : "clockbuttonclock")}.svg") };
    
    
    //[ObservableProperty] private Path _alarmButton;
    [ObservableProperty] private SvgImage _alarmButtonOff;
    [ObservableProperty] private SvgImage _alarmButtonOn;
    
    public ClockViewModel()
    {
        FrameInitializer();
        ClockInitializer();
        AlarmButtonInitializer();
        /*
         *
         *<Image Source="{Binding Imageitself}"/>
         *<Button Command="{Binding Path=ActivateAlarmButton}" Grid.Column="2" Margin="20,0,20,0" HorizontalAlignment="Center"  >
               <!-- <ContentControl  Content="{Binding Path=Imageitself}" /> -->
               
           </Button>
         *
         *<ContentControl PointerPressed="{Binding Path= DoTheThing}" Margin="50" Grid.Column="2" HorizontalContentAlignment="Right" HorizontalAlignment="Right" Content="{Binding Path=AlarmButton}" /> 
              
              <Button Command="{Binding Path=ActivateAlarmButton}" Grid.Column="2" Margin="20,0,20,0" HorizontalAlignment="Center"  >
               <!-- <ContentControl  Content="{Binding Path=Imageitself}" /> -->
               <Image Source="{Binding TimeImagenda}"/>
           </Button>   
                             
         *
         * 
         */
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
        
        /*AlarmButton = new Path()
        {
            Stroke =  new SolidColorBrush(Color.FromArgb(255, 0xff, 0x90, 0x1b)),
            StrokeThickness = 10,
            //Margin = new Thickness(),
            Stretch = Stretch.Uniform,
            Data = new PathGeometry()
            {
                Figures = new ActivateAlarm().Rectangles,
            }
        }; */
    }




    public void ActivateAlarmButton()
    {
        //AlarmButton.SwitchImage();
        Console.WriteLine($"AlarmButton pressed");
        IsGoodMorning = !IsGoodMorning;

    }
}