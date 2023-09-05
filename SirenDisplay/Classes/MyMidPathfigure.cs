using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes;

public sealed class MyMidPathfigure : IMyPathfigure
{
    public Point StartPoint => new Point(20, 40);

    public LineSegment[] LineSegments => new[]
    {
        new LineSegment { Point = new Point(40,40) },
        new LineSegment { Point = new Point(50,50) },
        new LineSegment { Point = new Point(40,60) },
        new LineSegment { Point = new Point(20,60) },
        new LineSegment { Point = new Point(10,50) },
        new LineSegment { Point = new Point(20,40) },
    };

}