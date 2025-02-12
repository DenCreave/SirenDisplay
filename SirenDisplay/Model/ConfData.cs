using System;
using System.Collections.Generic;

namespace SirenDisplay.Model;

public sealed class ConfData
{//todo just wtf? how to store time etc
    public bool IsPending;
    public List<string> MusicPaths;
    public TimeSpan UsualTime; //like 7:30
    public DateTimeOffset NextSirenTime; //like 2025.02.24 7:30
}