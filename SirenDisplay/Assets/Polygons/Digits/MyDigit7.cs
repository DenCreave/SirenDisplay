using Avalonia.Media;
using SirenDisplay.Assets.Polygons.BasicHexaFigure;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.Polygons.Digits;

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