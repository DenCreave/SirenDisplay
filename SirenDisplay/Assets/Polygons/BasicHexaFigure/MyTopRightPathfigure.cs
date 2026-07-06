using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.Polygons.BasicHexaFigure;

public sealed class MyTopRightPathfigure : IMyPathfigure
{
    public PathFigure Figure => new PathFigure
    {
        StartPoint = new Point(40, 0),
        Segments = new PathSegments
        {
            new LineSegment { Point = new Point(40,30) },
            new LineSegment { Point = new Point(35,35) },
            new LineSegment { Point = new Point(30,30) },
            new LineSegment { Point = new Point(30,10) },
            new LineSegment { Point = new Point(40,0) }
        }
    };
    /*public Point StartPoint => new Point(50,10);

    public PathSegments Segments => new PathSegments
    {
        new LineSegment { Point = new Point(60,20) },
        new LineSegment { Point = new Point(60,40) },
        new LineSegment { Point = new Point(50,50) },
        new LineSegment { Point = new Point(40,40) },
        new LineSegment { Point = new Point(40,20) },
        new LineSegment { Point = new Point(50,10) }
    };*/

    /*public LineSegment[] LineSegments => new[]
    {
        new LineSegment { Point = new Point(60,20) },
        new LineSegment { Point = new Point(60,40) },
        new LineSegment { Point = new Point(50,50) },
        new LineSegment { Point = new Point(40,40) },
        new LineSegment { Point = new Point(40,20) },
        new LineSegment { Point = new Point(50,10) }
    };*/

}