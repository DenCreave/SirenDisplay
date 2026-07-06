using Avalonia.Media;
using SirenDisplay.Assets.Polygons.BasicHexaFigure;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.Polygons.Digits;

public sealed class MyDigit2 : IMyDigit
{
    public int ID => 2;
    public PathFigures PathFigures => new PathFigures{
        new MyTopPathFigure().Figure,
        new MyTopRightPathfigure().Figure,
        new MyMidPathfigure().Figure,
        new MyBottomLeftPathFigure().Figure,
        new MyBottomPathfigure().Figure
    };
}