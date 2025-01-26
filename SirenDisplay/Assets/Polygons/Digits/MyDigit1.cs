using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes.Digits;

public sealed class MyDigit1 : IMyDigit
{
    public int ID => 1;

    public PathFigures PathFigures => new PathFigures
    {
        new MyTopRightPathfigure().Figure,
        new MyBottomRightPathFigure().Figure
    };
}