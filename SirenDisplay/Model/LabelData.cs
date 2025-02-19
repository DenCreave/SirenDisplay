using SirenDisplay.Interfaces;

namespace SirenDisplay.Model;

public sealed class LabelData : ILabelData
{
    public string OffLabel => "\uE85E";

    public string[] AnimatingLabel =>
    [
        "\uE2B4", "\uE2B8", "\uE2B6"
    ];

    public string PendingLabel => "\uE492";
    public string SirenLabel => "\uE9B8";
    public string UpLabel => "\uE13C";
    public string DownLabel => "\uE136";
    public string DoubleRightLabel => "\uE12A";
    public string DoubleLeftLabel => "\uE128";
    public string PlayLabel => "\uE3D0";
    public string StopLabel => "\uE46C";
    public string SaveLabel => "\uE428";
    public string BackLabel => "\uE3D4";
    public string AddLabel => "\uE07E";
    public string EditLabel => "\uE3B2";
    public string DeleteLabel => "\uE4F6";
    public string LeftLabel => "\uE138";
    public string RightLabel => "\uE13A";
    public string FolderLabel => "\uE24A";
    public string MusicLabel => "\uE802";
    public string CheckLabel => "\uE182";
        
}