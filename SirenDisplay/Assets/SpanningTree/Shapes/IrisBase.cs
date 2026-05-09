using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.Shapes;

public class IrisBase : IVexShape
{
    // gonna be chained together.
    public Vertex[] Shapes { get; init; } =
    [
        new Vertex() { Cox = 0, Coy = 5 },
        new Vertex() { Cox = 0, Coy = -5 },
    ];
}