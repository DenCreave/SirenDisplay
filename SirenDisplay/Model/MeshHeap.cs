using System.Collections.Generic;

namespace SirenDisplay.Model;

public sealed class MeshHeap
{
    public List<Vertex> RootNodes { get; set; } = new();
    public List<VexEdge> Edges { get; set; } = new();
    
}