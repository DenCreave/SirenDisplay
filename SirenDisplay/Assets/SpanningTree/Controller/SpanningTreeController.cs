using System.Collections.Generic;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.Controller;

public sealed class SpanningTreeController
{
    public Dictionary<DMGroup, IDotMapFactory[]>  DotMaps { get;} = new DotMapLoader().DotMaps;
    public Dictionary<TLGroup, ITorrentLayer[]> TorrentLayers { get; } = new TorrentLayerLoader().TorrentLayers;
    public DMGroup CurrentDMGroup { get; set; } = DMGroup.Eye;
    public TLGroup CurrentTLGroup { get; set; } = TLGroup.Eye;
}