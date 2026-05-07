using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.ShapeProps;

namespace SirenDisplay.Assets.SpanningTree.Shapes;

public sealed class Star : IVexShape<StarProperties>
{
    public Vertex[] Shapes => new Vertex[]
    {
        Nil,
        new Vertex()
        {
            Cox = -1,
            Coy = -1
        },
        new Vertex()
        {
            Cox = -1,
            Coy = -1
        },
        new Vertex()
        {
            Cox = -0.5,
            Coy = -2
        },
        new Vertex()
        {
            Cox = 0,
            Coy = -5
        },
        new Vertex()
        {
            Cox = 0.5,
            Coy = -2
        },
        new Vertex()
        {
            Cox = 1,
            Coy = -1
        },
        End,
        new Vertex()
        {
            Cox = 1,
            Coy = 1
        },
        new Vertex()
        {
            Cox = 0.5,
            Coy = 2
        },
        new Vertex()
        {
            Cox = 0,
            Coy = 5
        },
        new Vertex()
        {
            Cox = -0.5,
            Coy = 2
        },
        new Vertex()
        {
            Cox = -1,
            Coy = 1
        },
    };

    public Vertex Nil { get; set; } = new Vertex()
    {
        Cox = -2,
        Coy = 0
    };

    
    
    public Vertex End { get; set; } = new Vertex()
    {
        Cox = 2,
        Coy = 0
    };

    public StarProperties UniqueProps { get; }
}