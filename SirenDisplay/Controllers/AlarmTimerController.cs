using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Model;

namespace SirenDisplay.Controllers;

public sealed partial class AlarmTimerController : ObservableObject
{
    public Timer? Timer { get; set; }
    public DateTimeOffset SirenTime { get; set; }

    public ConfData SirenData { get; set; }

    public AudioController AudioController { get; set; }
    public LabelData LabelData { get; }
    public string Kolor => IsOff ? "#ff901b" : "#9e0022";
    
    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(Kolor))] private bool _isOff;
    
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

    public AlarmTimerController()
    {
        IsOff = true;
        LabelData = new LabelData();
        SirenData = ConfController.LoadConf();
        AudioController = new AudioController();
        if (SirenData.IsPending)
        {
            Start();
        }
    }

    public void Start()
    {
        DateTimeOffset tmpTime = DateTimeOffset.Now;
        TimeSpan dueTimeSpan;
        TimeSpan nowSpan = new TimeSpan(tmpTime.Hour, tmpTime.Minute, tmpTime.Second);
        if (!(nowSpan < SirenData.UsualTime))
        {
            //add a day so if its new years or new month it would still correctly work
            tmpTime = tmpTime.AddDays(1);
        }

        SirenTime = new DateTimeOffset(tmpTime.Year, tmpTime.Month, tmpTime.Day,
            SirenData.UsualTime.Hours, SirenData.UsualTime.Minutes,
            SirenData.UsualTime.Seconds, tmpTime.Offset);

        dueTimeSpan = SirenTime - DateTimeOffset.Now;

        Console.WriteLine("duetimesapn " + dueTimeSpan);

        if (Timer != null)
        {
            Stop();
        }
        

        Timer = new Timer(SirenMe, null, dueTimeSpan, Timeout.InfiniteTimeSpan);
    }

    private async void SirenMe(object state)
    {
        AlarmState = AlarmState.Sirens;
        await AudioController.PlaySirenDisplay(SirenData.MusicPaths[SirenData.SelectedPlaylist]);
        Stop();
    }

    public void ActivateAlarmTimer()
    {
        if (IsOff)
        {
            IsOff = false;
            AlarmState = AlarmState.Pending;
            SirenData.IsPending = true;
            Start();
            ConfController.SaveConf(SirenData);
        }
        else
        {
            IsOff = true;
            AlarmState = AlarmState.Off;
            SirenData.IsPending = false;
            AudioController.Stop();
            Stop();
            ConfController.SaveConf(SirenData);
        }
    }
    public void Stop()
    {
        Timer?.Dispose();
        Console.WriteLine("for whatever reason, Timer.dispose, hope it got awaited");
    }
}