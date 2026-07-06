using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.Polygons.BasicHexaFigure;

public sealed class MyBottomPathfigure : IMyPathfigure
{
    public PathFigure Figure => new PathFigure
    {
        StartPoint = new Point(0,70),
        Segments = new PathSegments
        {
            new LineSegment { Point = new Point(10,60) },
            new LineSegment { Point = new Point(30,60) },
            new LineSegment { Point = new Point(40,70) },
            new LineSegment { Point = new Point(0,70) }
        }
    };

    // public Point StartPoint => new Point(20, 80);
    //
    // public LineSegment[] LineSegments => new[]
    // {
    //     new LineSegment { Point = new Point(40,80) },
    //     new LineSegment { Point = new Point(50,90) },
    //     new LineSegment { Point = new Point(40,100) },
    //     new LineSegment { Point = new Point(20,100) },
    //     new LineSegment { Point = new Point(10,90) },
    //     new LineSegment { Point = new Point(20,80) }
    // };
    
}