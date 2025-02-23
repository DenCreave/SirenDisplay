using System;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Model;

namespace SirenDisplay.Controllers;

public sealed partial class AlarmTimerController : ObservableObject
{
    private static readonly Lazy<AlarmTimerController> _instance = new Lazy<AlarmTimerController>(() => new AlarmTimerController());
    //private static readonly object _lock = new object();
    public static AlarmTimerController Instance => _instance.Value;
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

    private AlarmTimerController()
    {
        IsOff = true;
        LabelData = new LabelData();
        SirenData = ConfController.LoadConf();
        AudioController = new AudioController();
        //ArdentAlarm();
        Console.WriteLine("Alarm timer WAS INITIALIZED");
    }
    
    /*public AlarmTimerController()
    {
        IsOff = true;
        LabelData = new LabelData();
        SirenData = ConfController.LoadConf();
        AudioController = new AudioController();
        /*if (SirenData.IsPending)
        {
            //
            //Start();
            /*if (Timer != null)
            {
                Stop();
            }
            IsOff = false;
            AlarmState = AlarmState.Sirens;
            SirenMe(null);
            /// so there is a funny bug about this, cachreference, or mostly anything
            /// gets instantiated twice
            /// i dont know, why, or where, but if i do this in the constructor instead of calling it
            /// in the postload of ClockViewModel, then even after i close the program, the playlist keeps playing
            /// and i cant close it, i dont know where its running from, it just does
            /// im glad the testing playlist wasnt hours long
            /// as it turns out the answer was starting the music before the project is initialized
            /// and calling ArdentAlarm there
            /// and also making this a singleton
        }
    }*/

    public void ArdentAlarm()
    {
        //todo figure out to only play it once we are past the wake time, otherwise just start the timer
        if (SirenData.IsPending)
        {
            Console.WriteLine($"next siren time: {SirenData.NextSirenTime}, dateoffset now: {DateTimeOffset.Now}");
            if (SirenData.NextSirenTime < DateTimeOffset.Now)
            {
                
                Timer?.Dispose();
                Timer = null;
                IsOff = false;
                AlarmState = AlarmState.Sirens;
                SirenMe(null);
            }
            else
            {
                Start();
            }
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
        
        SirenData.IsPending = true;
        SirenData.NextSirenTime = SirenTime;
        Timer = new Timer(SirenMe, null, dueTimeSpan, Timeout.InfiniteTimeSpan);
        ConfController.SaveConf(SirenData);
    }

    private async void SirenMe(object state)
    {
        AlarmState = AlarmState.Sirens;
        if (SirenData.MusicPaths.ContainsKey(SirenData.SelectedPlaylist))
        {
            await AudioController.PlaySirenDisplay(SirenData.MusicPaths[SirenData.SelectedPlaylist]);
        }
        Stop();
    }

    public void ActivateAlarmTimer()
    {
        if (IsOff)
        {
            IsOff = false;
            AlarmState = AlarmState.Pending;
            Start();
        }
        else
        {
            IsOff = true;
            AlarmState = AlarmState.Off;
            //SirenData.IsPending = false;
            AudioController.Stop();
            Stop();
            
        }
    }
    public void Stop()
    {
        Timer?.Dispose();
        SirenData.IsPending = false;
        SirenData.NextSirenTime = null;
        ConfController.SaveConf(SirenData);
        Console.WriteLine("Timer.dispose was called");
    }
}