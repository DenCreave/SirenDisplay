using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.ShapeProps;

namespace SirenDisplay.SpanningTree.Shapes;

public sealed class Star : IVexShape<StarProperties>
{
    public Vertex[] Shapes { get; init; }
    public StarProperties UniqueProps { get; }


    public Star()
    {
        UniqueProps = new StarProperties()
        {
            Nil = new Vertex()
            {
                Cox = -2,
                Coy = 0
            },
            End = new Vertex()
            {
                Cox = 2,
                Coy = 0
            }
        };
        
        Shapes =
        [
            UniqueProps.Nil,
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
            UniqueProps.End,
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
            }
        ];
    }
}