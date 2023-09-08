using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes.Digits;

public sealed class MyDigit5 : IMyDigit
{
    public int ID => 5;

    public PathFigures PathFigures => new PathFigures()
    {
        new MyTopPathFigure().Figure,
        new MyTopLeftPathfigure().Figure,
        new MyMidPathfigure().Figure,
        new MyBottomRightPathFigure().Figure,
        new MyBottomPathfigure().Figure
    };
}