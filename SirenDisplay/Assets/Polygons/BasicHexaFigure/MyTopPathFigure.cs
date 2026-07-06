using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.Polygons.BasicHexaFigure;

public sealed class MyTopPathFigure : IMyPathfigure
{
   // public Point StartPoint => new Point(20, 0);

    public PathFigure Figure => new PathFigure
    {
        StartPoint = new Point(0, 0),
        Segments = new PathSegments()
        {
            new LineSegment { Point = new Point(40, 0) },
            new LineSegment { Point = new Point(30, 10) },
            new LineSegment { Point = new Point(10, 10) },
            new LineSegment { Point = new Point(0, 0) }
        }
    };
    /*public PathSegments Segments => new PathSegments
    {
        new LineSegment { Point = new Point(40, 0) },
        new LineSegment { Point = new Point(50, 10) },
        new LineSegment { Point = new Point(40, 20) },
        new LineSegment { Point = new Point(20, 20) },
        new LineSegment { Point = new Point(10, 10) },
        new LineSegment { Point = new Point(20, 0) }
    };*/

    /*public LineSegment[] LineSegments => new[]
    {
        new LineSegment { Point = new Point(40,0) },
        new LineSegment { Point = new Point(50,10) },
        new LineSegment { Point = new Point(40,20) },
        new LineSegment { Point = new Point(20,20) },
        new LineSegment { Point = new Point(10,10) },
        new LineSegment { Point = new Point(20,0) }
    };*/


}