using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes.Digits;

public sealed class MyDigit8 : IMyDigit
{
    public int ID => 8;

    public PathFigures PathFigures => new PathFigures()
    {
        new MyTopPathFigure().Figure,
        new MyTopLeftPathfigure().Figure,
        new MyTopRightPathfigure().Figure,
        new MyMidPathfigure().Figure,
        new MyBottomPathfigure().Figure,
        new MyBottomLeftPathFigure().Figure,
        new MyBottomRightPathFigure().Figure
    };
}