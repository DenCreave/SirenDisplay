using SirenDisplay.Assets.SpanningTree.Theme;

namespace SirenDisplay.Interfaces;

public interface IThemeBinder
{
    ThemeGroup ThemeGroup { get; }
    Animap[] ElicitateAnimap();
}