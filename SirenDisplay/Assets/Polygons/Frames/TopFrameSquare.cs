using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.Polygons.Frames;

public sealed class TopFrameSquare : IFrames
{
    public string Name => "TopFrameSquare";

    public PathFigure PathFigure => new PathFigure
    {
        StartPoint = new Point(0, 10),
        Segments = new PathSegments()
        {
            new LineSegment { Point = new Point(80, 10) },
            new LineSegment { Point = new Point(80, 0) },
            new LineSegment { Point = new Point(0, 0) }
        }
    };
}