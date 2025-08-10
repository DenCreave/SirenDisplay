using System.Collections.Generic;
using System.Security.AccessControl;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.DotMap;

public sealed class DefaultDotMap : IDotMap
{
    public string Name => "Default";
    public int ID => 0;
    public bool IsStatic => true;
    public  HashSet<Vertex> Vertices =new();
    
    public void IncreaseDots()
    {
        throw new System.NotImplementedException();
    }

    public void DecreaseDots()
    {
        throw new System.NotImplementedException();
    }
}