using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.Polygons.Frames;

public sealed class BottomFrame :IFrames
{
    public string Name => "BottomFrame";

    public PathFigure PathFigure => new PathFigure
    {
        StartPoint = new Point(0, 38),
        Segments = new PathSegments()
        {
            new LineSegment { Point = new Point(0, 48) },
            new LineSegment { Point = new Point(70, 48) },
            new LineSegment { Point = new Point(80, 38) }
        }
    };
}