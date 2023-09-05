using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes;

public sealed class MyBottomPathfigure : IMyPathfigure
{
    public Point StartPoint => new Point(20, 80);

    public LineSegment[] LineSegments => new[]
    {
        new LineSegment { Point = new Point(40,80) },
        new LineSegment { Point = new Point(50,90) },
        new LineSegment { Point = new Point(40,100) },
        new LineSegment { Point = new Point(20,100) },
        new LineSegment { Point = new Point(10,90) },
        new LineSegment { Point = new Point(20,80) }
    };
    
}