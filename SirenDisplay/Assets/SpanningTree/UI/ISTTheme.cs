using Avalonia.Media;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.UI;

public interface ISTTheme
{
    ThemeGroup ThemeGroup { get; }
    StyleArchive StyleArchive { get; }
    public void Draw(DrawingContext context, Animap[] currentScene);
}