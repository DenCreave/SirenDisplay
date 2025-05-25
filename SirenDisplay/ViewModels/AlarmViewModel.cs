using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
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
    
    public AlarmViewModel()
    {
        //todo constructor, tho might not even need it
        //oh yea, init the mainframe and observeable digits
        _digitLoader = new DigitLoader();
        LabelData = new LabelData();
        InitDefaultDigits();
        FrameInitializer();
        
    }

    private void InitDefaultDigits()
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
        HourDigit = GeneratePathDefaults();
        HourDecimalDigit = GeneratePathDefaults();
        MinuteDigit = GeneratePathDefaults();
        MinuteDecimalDigit = GeneratePathDefaults();
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
                    new BottomFrameSquare().PathFigure
                }
            },
            Effect = Application.Current.FindResource("OffEffect") as DropShadowEffect
        };
    }
    
    public void LoadIATC()
    {
        IATCHours = CacheReferences.alarmTimerController.SirenData.UsualTime.Hours; //damn its so long
        IATCHoursDecimal = IATCHours / 10;
        IATCHours= IATCHours % 10;
        IATCMinutes = CacheReferences.alarmTimerController.SirenData.UsualTime.Minutes;
        IATCMinutesDecimal = IATCMinutes / 10;
        IATCMinutes = IATCMinutes % 10;
        
        MinuteDigit.Data = _digitLoader.ReturnPathGeometry(IATCMinutes);
        MinuteDecimalDigit.Data = _digitLoader.ReturnPathGeometry(IATCMinutesDecimal);
        HourDigit.Data = _digitLoader.ReturnPathGeometry(IATCHours);
        HourDecimalDigit.Data = _digitLoader.ReturnPathGeometry(IATCHoursDecimal);
    }
    

    public void SaveIATC()
    {
        TimeSpan noni= new TimeSpan(IATCHoursDecimal*10+IATCHours, IATCMinutesDecimal*10+IATCMinutes, 0);
        Console.WriteLine("the timespan created after saving in SaveIATC is: "+noni.ToString());
        CacheReferences.alarmTimerController.SirenData.UsualTime = noni;
        ConfController.SaveConf(CacheReferences.alarmTimerController.SirenData);
    }

    //todo continue from here:hours minutes decimal to show up on ui with digitloader and such
    #region IAlarmTimeController
    public int IATCHours { get; set; }
    public int IATCHoursDecimal { get; set; }
    public int IATCMinutes { get; set; }
    public int IATCMinutesDecimal { get; set; }

    public void IncreaseMinute()
    {
        ++IATCMinutes;
        IATCMinutes %= 10;
        MinuteDigit.Data = _digitLoader.ReturnPathGeometry(IATCMinutes);
    }

    public void DecreaseMinute()
    {
        --IATCMinutes;
        if (IATCMinutes == -1)
        {
            IATCMinutes = 9;
        }
        MinuteDigit.Data = _digitLoader.ReturnPathGeometry(IATCMinutes);
    }

    public void IncreaseMinuteDecimal()
    {
        ++IATCMinutesDecimal;
        IATCMinutesDecimal %= 6;
        MinuteDecimalDigit.Data = _digitLoader.ReturnPathGeometry(IATCMinutesDecimal);
    }

    public void DecreaseMinuteDecimal()
    {
        --IATCMinutesDecimal;
        if (IATCMinutesDecimal == -1)
        {
            IATCMinutesDecimal = 5;
        }
        MinuteDecimalDigit.Data = _digitLoader.ReturnPathGeometry(IATCMinutesDecimal);
    }

    public void IncreaseHour()
    {
        ++IATCHours;
        IATCHours %= IATCHoursDecimal>1?4:10;
        HourDigit.Data = _digitLoader.ReturnPathGeometry(IATCHours);
    }

    public void DecreaseHour()
    {
        --IATCHours;
        if (IATCHours == -1)
        {
            IATCHours = IATCHoursDecimal>1?3:9;
        }
        HourDigit.Data = _digitLoader.ReturnPathGeometry(IATCHours);
    }

    public void IncreaseHourDecimal()
    {
        ++IATCHoursDecimal;
        IATCHoursDecimal %= 3;
        
        HourDecimalDigit.Data = _digitLoader.ReturnPathGeometry(IATCHoursDecimal);
        if (IATCHours>3 && IATCHoursDecimal > 1)
        {
            IATCHours = 3;
            HourDigit.Data = _digitLoader.ReturnPathGeometry(IATCHours);
        }
    }

    public void DecreaseHourDecimal()
    {
        --IATCHoursDecimal;
        if (IATCHoursDecimal == -1)
        {
            IATCHoursDecimal = 2;
        }
        HourDecimalDigit.Data = _digitLoader.ReturnPathGeometry(IATCHoursDecimal);
        if (IATCHours>3 && IATCHoursDecimal > 1)
        {
            IATCHours = 3;
            HourDigit.Data = _digitLoader.ReturnPathGeometry(IATCHours);
        }
    }
    #endregion IAlarmTimeController
    
    //todo make a save and exit button and function, then i can start testing this page
}