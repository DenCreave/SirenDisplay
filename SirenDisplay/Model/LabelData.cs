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
    public string RightLabel => "\uE12A";
    public string LeftLabel => "\uE128";
    public string PlayLabel => "\uE3D0";
    public string StopLabel => "\uE46C";
    public string ExitUpleftLabel => "\uE090";
    public string ExitDownRightLabel => "\uE042";
}