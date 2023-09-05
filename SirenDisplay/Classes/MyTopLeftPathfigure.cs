using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes;

public sealed class MyTopLeftPathfigure : IMyPathfigure
{
    public Point StartPoint => new Point(10, 10);

    public LineSegment[] LineSegments => new[]
    {
        new LineSegment { Point = new Point(20,20) },
        new LineSegment { Point = new Point(20,40) },
        new LineSegment { Point = new Point(10,50) },
        new LineSegment { Point = new Point(0,40) },
        new LineSegment { Point = new Point(0,20) },
        new LineSegment { Point = new Point(10,10) }
    };
    
}