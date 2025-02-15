namespace SirenDisplay.Interfaces;
/// <summary>
/// at alarmview it controls the numbers on the clock
/// </summary>
public interface IAlarmTimeController
{
    public int IATCHours { get; set; }
    public int IATCMinutes { get; set; }
    public void LoadIATC();
    public void SaveIATC();
    public void IncreaseMinute();
    public void DecreaseMinute();
    public void IncreaseMinuteDecimal();
    public void DecreaseMinuteDecimal();
    public void IncreaseHour();
    public void DecreaseHour();
    public void IncreaseHourDecimal();
    public void DecreaseHourDecimal();
}