using System.Collections.Generic;

namespace SirenDisplay.Model;

public sealed class MeshHeap
{
    public HashSet<Vertex> RootNodes => new();
    public Vertex Current { get; set; }
}