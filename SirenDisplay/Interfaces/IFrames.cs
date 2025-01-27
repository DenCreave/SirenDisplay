using Avalonia.Media;

namespace SirenDisplay.Interfaces;

public interface IFrames
{
    public string Name { get; }
    public PathFigure PathFigure { get; }
}