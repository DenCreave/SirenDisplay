using Avalonia.Media;
using SirenDisplay.SpanningTree.Theme;

namespace SirenDisplay.SpanningTree.UI;

public interface ISTTheme
{
    ThemeGroup ThemeGroup { get; }
    StyleArchive StyleArchive { get; }
    public void Draw(DrawingContext context, Animap[] currentScene);
}