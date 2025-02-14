using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Channels;
using Avalonia.Markup.Xaml;
using SirenDisplay.Model;

namespace SirenDisplay.Controllers;

/// <summary>
/// controls the json config that contains the alarm state, time, and music list 
/// </summary>
public sealed class ConfController
{
    
    private string _confPath => "conf.json";

    public void SaveConf(ConfData data)
    {
        string serializeMe = JsonSerializer.Serialize(data);
        File.WriteAllText(_confPath, serializeMe);
    }

    public ConfData LoadConf()
    {
        ConfData data = new ConfData();
        if (File.Exists(_confPath))
        {
            data = JsonSerializer.Deserialize<ConfData>(File.ReadAllText(_confPath));
            Console.WriteLine($"is data null? {data == null}");
        }
        else
        {
            Console.WriteLine("no conf.json found");
            data = new ConfData()
            {
                IsPending = false,
                MusicPaths = new List<string>(),
                UsualTime = new TimeSpan(17,0,0),
                NextSirenTime = null 
            };
        }

        return data;
    }
}