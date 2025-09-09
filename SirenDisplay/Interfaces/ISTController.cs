using SirenDisplay.Assets.SpanningTree.DotMap.Controller;

namespace SirenDisplay.Interfaces;

public interface ISTController
{
    STCGroup Group { get; }
    IDotMap DotMap { get; }
    ITorrentLayer Layer { get; }
    //todo functions
}