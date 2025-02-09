using SirenDisplay.ViewModels;

namespace SirenDisplay.Model;

public sealed class CacheVM
{
    public InitViewModel InitViewModel  { get; set; }
    public ClockViewModel clockViewModel { get; set; }
    public AlarmViewModel alarmViewModel { get; set; }
}