using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Model;

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