using Avalonia.Media;

namespace SirenDisplay.Assets.Polygons.Frames;

public interface IFrames
{
    public string Name { get; }
    public PathFigure PathFigure { get; }
}