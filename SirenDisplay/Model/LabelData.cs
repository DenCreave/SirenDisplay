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
}