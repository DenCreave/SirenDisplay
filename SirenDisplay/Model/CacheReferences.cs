using SirenDisplay.Controllers;
using SirenDisplay.ViewModels;

namespace SirenDisplay.Model;

public sealed class CacheReferences
{
    public InitViewModel InitViewModel  { get; set; }
    public AlarmTimerController alarmTimerController { get; set; }
}