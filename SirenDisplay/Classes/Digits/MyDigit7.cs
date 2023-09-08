using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes.Digits;

public sealed class MyDigit7 : IMyDigit
{
    public int ID => 7;
    
    public PathFigures PathFigures => new PathFigures()
    {
        new MyTopPathFigure().Figure,
        new MyTopRightPathfigure().Figure,
        new MyBottomRightPathFigure().Figure
    };
}