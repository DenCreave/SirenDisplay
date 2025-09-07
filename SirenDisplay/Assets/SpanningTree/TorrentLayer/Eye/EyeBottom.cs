using System.Collections.Generic;
using System.Collections.ObjectModel;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeBottom : ITorrentLayer
{
    public TLGroup Group => TLGroup.Eye;
    public string Name => "Bottom";
   // public List<Vertex> TorrentPath { get; }
}