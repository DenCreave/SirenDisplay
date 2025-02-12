using System;
using System.Threading;

namespace SirenDisplay.Controllers;

public sealed class AlarmTimerController
{
    
    public Timer? Timer { get; set; }
    public DateTimeOffset SirenTime { get; set; }

    public TimeSpan LoadConf()
    {
        //return new TimeSpan(7, 36, 30); //todo load conf
        throw new NotImplementedException("bruh you forgot something: AlarmTimerController.LoadConf()");
    }

    public void Start()
    {
        DateTimeOffset tmpTime = DateTimeOffset.Now;
        

        TimeSpan dueTimeSpan;
        TimeSpan nowSpan = new TimeSpan(tmpTime.Hour, tmpTime.Minute, tmpTime.Second);
        TimeSpan AlarmTime = LoadConf();
        
        if (!(nowSpan < AlarmTime))
        {
            //add a day so if its new years or new month it would still correctly work
            tmpTime = tmpTime.AddDays(1);
        }
        SirenTime = new DateTimeOffset(tmpTime.Year, tmpTime.Month, tmpTime.Day,
            AlarmTime.Hours, AlarmTime.Minutes,
            AlarmTime.Seconds, tmpTime.Offset); //todo check if this offset works
        
        dueTimeSpan = SirenTime - DateTimeOffset.Now;
        
        Console.WriteLine("duetimesapn "+dueTimeSpan);
        
        if (Timer != null)
        {
            Stop();
        }


        Timer = new Timer(SirenMe, null,dueTimeSpan, Timeout.InfiniteTimeSpan);
        

    }

    private void SirenMe(object state)
    {
        throw new NotImplementedException("implement playing music"); //async task
        Stop();
    }
    
    public void Stop()
    {
        Timer?.Dispose();
    }
}