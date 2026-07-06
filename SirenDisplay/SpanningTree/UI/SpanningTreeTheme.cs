using Avalonia.Media;
using SirenDisplay.SpanningTree.Theme;

namespace SirenDisplay.SpanningTree.UI;

public sealed class SpanningTreeTheme(UIThemeLoader loader)
{
    private readonly UIThemeLoader _loader = loader;
    
    public ISTTheme CurrentTheme { get; set; }

    public void LoadTheme(ThemeGroup group)
    {
        CurrentTheme = _loader.UIThemeDict[group];
    }
    
    public void DrawTheme(DrawingContext context, Animap[] layers, ThemeGroup currentTheme)
    {
        
        if (CurrentTheme is null || CurrentTheme.ThemeGroup != currentTheme)
        {
            LoadTheme(currentTheme);
        }

        CurrentTheme.Draw(context, layers);
    }
}