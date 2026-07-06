using Avalonia.Media;
using SirenDisplay.Assets.Polygons.BasicHexaFigure;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.Polygons.Digits;

public sealed class MyDigit1 : IMyDigit
{
    public int ID => 1;

    public PathFigures PathFigures => new PathFigures
    {
        new MyTopRightPathfigure().Figure,
        new MyBottomRightPathFigure().Figure
    };
}