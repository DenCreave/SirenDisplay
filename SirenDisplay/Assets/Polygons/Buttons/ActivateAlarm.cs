using Avalonia;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.Polygons.Buttons;

public sealed class ActivateAlarm 
{
    public string Name => "Activate Alarm";

    public PathFigures Rectangles => new PathFigures()
    {
        new PathFigure()
        {
            StartPoint = new Point(10, 10),
            Segments = new PathSegments()
            {
                new LineSegment { Point = new Point(30, 10) },
                new LineSegment { Point = new Point(30, 30) },
                new LineSegment { Point = new Point(10, 30) }
            }
        },
        new PathFigure()
        {
            StartPoint = new Point(10, 40),
            Segments = new PathSegments()
            {
                new LineSegment { Point = new Point(30, 40) },
                new LineSegment { Point = new Point(30, 60) },
                new LineSegment { Point = new Point(10, 60) }
            }
        }
    };
}