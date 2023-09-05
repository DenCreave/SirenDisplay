using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes;

public sealed class MyBottomRightPathFigure : IMyPathfigure
{
    public Point StartPoint => new Point(50, 50);

    public LineSegment[] LineSegments => new[]
    {
        new LineSegment { Point = new Point(60,60) },
        new LineSegment { Point = new Point(60,80) },
        new LineSegment { Point = new Point(50,90) },
        new LineSegment { Point = new Point(40,80) },
        new LineSegment { Point = new Point(40,60) },
        new LineSegment { Point = new Point(50,50) }
    };
    
}