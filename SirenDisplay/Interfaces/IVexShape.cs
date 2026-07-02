using Avalonia.Media;
using SirenDisplay.Model;

namespace SirenDisplay.Interfaces;

public interface IVexShape
{
    public Vertex[] Shapes { get; init; }
}

public interface IVexShape<T> : IVexShape
    where T : IShapeProperties
{
    public T UniqueProps { get; }
}