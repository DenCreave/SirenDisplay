using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes;

public class MyBottomLeftPathFigure : IMyPathfigure
{
    public Point StartPoint => new Point(10, 50);

    public LineSegment[] LineSegments => new[]
    {
        new LineSegment { Point = new Point(20, 60) },
        new LineSegment { Point = new Point(20, 80) },
        new LineSegment { Point = new Point(10, 90) },
        new LineSegment { Point = new Point(0, 80) },
        new LineSegment { Point = new Point(0, 60) },
        new LineSegment { Point = new Point(10, 50) },
    };
    
}