using Avalonia;
using Avalonia.Media;

namespace SirenDisplay.Assets.Polygons.Frames;

public sealed class TopFrame : IFrames
{
    public string Name => "TopFrame";

    public PathFigure PathFigure => new PathFigure
    {
        StartPoint = new Point(0, 10),
        Segments = new PathSegments()
        {
            new LineSegment { Point = new Point(80, 10) },
            new LineSegment { Point = new Point(80, 0) },
            new LineSegment { Point = new Point(10, 0) }
        }
    };
}