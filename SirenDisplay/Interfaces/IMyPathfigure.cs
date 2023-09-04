using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;

namespace SirenDisplay.Interfaces;

public interface IMyPathfigure
{
    public Point StartPoint { get; }
    public LineSegment[] LineSegments { get; } 

}