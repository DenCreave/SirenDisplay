using System;
using System.Collections.Generic;

namespace SirenDisplay.Model;

public sealed class ConfData
{
    public bool IsPending { get; set; }
    public Dictionary<string , List<string>> MusicPaths { get; set;} //key is the title of the playlist
    public TimeSpan UsualTime { get; set; }//like 7:30
    public DateTimeOffset? NextSirenTime { get; set; } //like 2025.02.24 7:30
}