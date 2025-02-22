using SirenDisplay.Model;

namespace SirenDisplay.Interfaces;

public interface ILabelData
{
    public string OffLabel { get; }
    public string[] AnimatingLabel { get; }
    public string PendingLabel { get; }
    public string SirenLabel { get; }
    public string UpLabel { get; }
    public string DownLabel { get; }
    public string DoubleUpLabel { get; }
    public string DoubleRightLabel { get; }
    public string DoubleLeftLabel { get; }
    public string PlayLabel { get; }
    public string StopLabel { get; }
    public string SaveLabel { get; }
    public string BackLabel { get; }
    public string AddLabel { get; }
    public string EditLabel { get; }
    public string DeleteLabel { get; }
    public string LeftLabel { get; }
    public string RightLabel { get; }
    public string FolderLabel { get; }
    public string MusicLabel { get; }
    public string CheckLabel { get; }
}