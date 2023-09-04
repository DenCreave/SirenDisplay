using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes;

public sealed class MyTopPathFigure : IMyPathfigure
{
    public Point StartPoint => new Point(20, 0);

    public LineSegment[] LineSegments => new[]
    {
        new LineSegment { Point = new Point(40,0) },
        new LineSegment { Point = new Point(50,10) },
        new LineSegment { Point = new Point(40,20) },
        new LineSegment { Point = new Point(20,20) },
        new LineSegment { Point = new Point(10,10) },
        new LineSegment { Point = new Point(20,0) },
        //todo innen folytatni, megvan 
    };
    

}