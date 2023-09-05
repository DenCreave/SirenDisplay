using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes;

public sealed class MyTopRightPathfigure : IMyPathfigure
{
    public Point StartPoint => new Point(50,10);

    public LineSegment[] LineSegments => new[]
    {
        new LineSegment { Point = new Point(60,20) },
        new LineSegment { Point = new Point(60,40) },
        new LineSegment { Point = new Point(50,50) },
        new LineSegment { Point = new Point(40,40) },
        new LineSegment { Point = new Point(40,20) },
        new LineSegment { Point = new Point(50,10) }
    };

}