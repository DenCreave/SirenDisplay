using Avalonia.Media;
using SirenDisplay.Assets.SpanningTree.UI.Eye;

namespace SirenDisplay.Assets.SpanningTree.UI;

public sealed class StyleArchive(EyeStyle eyeStyle)
{
    public EyeStyle EyeStyle { get; } = eyeStyle;

    //todo brushes pens and colors come here

}