using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.DotMap;

public sealed class DefaultDotMap : IDotMap
{
    public string Name => "Default";
    public bool IsStatic => true;
    public void IncreaseDots()
    {
        throw new System.NotImplementedException();
    }

    public void DecreaseDots()
    {
        throw new System.NotImplementedException();
    }
}