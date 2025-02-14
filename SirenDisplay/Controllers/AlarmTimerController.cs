using System;
using System.Threading;
using SirenDisplay.Model;

namespace SirenDisplay.Controllers;

public sealed class AlarmTimerController
{
    
    public Timer? Timer { get; set; }
    public DateTimeOffset SirenTime { get; set; }
    
    public ConfData SirenData { get; set; }

    public AlarmTimerController()
    {
        SirenData = new ConfController().LoadConf();
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
        
        Console.WriteLine("duetimesapn "+dueTimeSpan);
        
        if (Timer != null)
        {
            Stop();
        }


        Timer = new Timer(SirenMe, null,dueTimeSpan, Timeout.InfiniteTimeSpan);
        

    }

    private void SirenMe(object state)
    {//todo implement music
        throw new NotImplementedException("implement playing music"); //async task
        Stop();
    }
    
    public void Stop()
    {
        Timer?.Dispose();
    }
}