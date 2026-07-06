using Avalonia.Media;
using SirenDisplay.Assets.Polygons.BasicHexaFigure;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.Polygons.Digits;

public sealed class MyDigit6 : IMyDigit
{
    public int ID => 6;

    public PathFigures PathFigures => new PathFigures()
    {
        new MyTopPathFigure().Figure,
        new MyTopLeftPathfigure().Figure,
        new MyBottomLeftPathFigure().Figure,
        new MyBottomPathfigure().Figure,
        new MyBottomRightPathFigure().Figure,
        new MyMidPathfigure().Figure
    };
}