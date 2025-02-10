using SirenDisplay.Model;

namespace SirenDisplay.Interfaces;

public interface ILabelData
{
    public string OffLabel {get;}
    public string[] AnimatingLabel {get;}
    public string PendingLabel {get;}
    public string SirenLabel {get;}
    public string UpLabel {get;}
    public string DownLabel {get;}
    public string RightLabel {get;}
    public string LeftLabel {get;}
    public string PlayLabel {get;}
    public string StopLabel {get;}
    public string ExitUpleftLabel {get;}
    public string ExitDownRightLabel {get;}
}