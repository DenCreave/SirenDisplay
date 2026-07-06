using Avalonia.Media;
using SirenDisplay.Assets.Polygons.BasicHexaFigure;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.Polygons.Digits;

public sealed class MyDigit0 : IMyDigit
{
    public int ID => 0;

    public PathFigures PathFigures => new PathFigures
    {
        new MyTopPathFigure().Figure,
        new MyTopLeftPathfigure().Figure,
        new MyTopRightPathfigure().Figure,
        new MyBottomPathfigure().Figure,
        new MyBottomLeftPathFigure().Figure,
        new MyBottomRightPathFigure().Figure
    };
}