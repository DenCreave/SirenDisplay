using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes.Digits;

public sealed class MyDigit3 : IMyDigit
{
    public int ID => 3;

    //note itt meghagytam a (), dunno mért lehet elengedni
    public PathFigures PathFigures => new PathFigures()
    {
        new MyTopPathFigure().Figure,
        new MyTopRightPathfigure().Figure,
        new MyMidPathfigure().Figure,
        new MyBottomRightPathFigure().Figure,
        new MyBottomPathfigure().Figure
    };
}