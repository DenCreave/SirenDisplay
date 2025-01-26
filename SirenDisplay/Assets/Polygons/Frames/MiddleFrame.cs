using Avalonia;
using Avalonia.Media;

namespace SirenDisplay.Assets.Polygons.Frames;

public sealed class MiddleFrame : IFrames
{
    public string Name => "MiddleFrame";

    public PathFigure PathFigure => new PathFigure
    {
        StartPoint = new Point(0, 10),
        Segments = new PathSegments()
        {
            new LineSegment { Point = new Point(0, 38) },
            new LineSegment { Point = new Point(80, 38) },
            new LineSegment { Point = new Point(80, 10) }
        }
    };
}