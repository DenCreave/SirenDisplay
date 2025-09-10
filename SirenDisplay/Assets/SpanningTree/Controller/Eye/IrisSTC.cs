using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.Controller.Eye;

public sealed class IrisSTC : ISTController
{
    public STCGroup Group => STCGroup.Eye;
    public STCName Name => STCName.Iris;
}