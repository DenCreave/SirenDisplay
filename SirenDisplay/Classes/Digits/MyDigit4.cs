using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes.Digits;

public sealed class MyDigit4 : IMyDigit
{
    public int ID => 4;

    public PathFigures PathFigures => new PathFigures()
    {
        new MyTopLeftPathfigure().Figure,
        new MyTopRightPathfigure().Figure,
        new MyMidPathfigure().Figure,
        new MyBottomRightPathFigure().Figure
    };
}