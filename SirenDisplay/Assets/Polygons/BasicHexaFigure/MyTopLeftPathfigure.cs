using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes;

public sealed class MyTopLeftPathfigure : IMyPathfigure
{
    public PathFigure Figure => new PathFigure
    {
        StartPoint = new Point(0, 0),
        Segments = new PathSegments
        {
            new LineSegment { Point = new Point(10, 10) },
            new LineSegment { Point = new Point(10, 30) },
            new LineSegment { Point = new Point(5, 35) },
            new LineSegment { Point = new Point(0, 30) },
            new LineSegment { Point = new Point(0, 0) }
        }
    };


    /*public Point StartPoint => new Point(10, 10);

    public PathSegments Segments => new PathSegments
    {
        new LineSegment { Point = new Point(20, 20) },
        new LineSegment { Point = new Point(20, 40) },
        new LineSegment { Point = new Point(10, 50) },
        new LineSegment { Point = new Point(0, 40) },
        new LineSegment { Point = new Point(0, 20) },
        new LineSegment { Point = new Point(10, 10) }
    };*/

    // public LineSegment[] LineSegments => new[]
    // {
    //     new LineSegment { Point = new Point(20,20) },
    //     new LineSegment { Point = new Point(20,40) },
    //     new LineSegment { Point = new Point(10,50) },
    //     new LineSegment { Point = new Point(0,40) },
    //     new LineSegment { Point = new Point(0,20) },
    //     new LineSegment { Point = new Point(10,10) }
    // };
    
}