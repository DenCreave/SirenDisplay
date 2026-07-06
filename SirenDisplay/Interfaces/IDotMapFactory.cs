using SirenDisplay.Model;
using SirenDisplay.SpanningTree.DotMap;
using SirenDisplay.SpanningTree.Theme;

namespace SirenDisplay.Interfaces;

public interface IDotMapFactory
{
    ThemeGroup Group { get; }
    DMName Name { get; } 
    int LayerLevel { get; } //to set an order
    Constellation GenerateVertices();
}

public interface IDotMapFactory<T> : IDotMapFactory
    where T : IDMProperties
{
    T UniqueProps { get; }
}