using System.Collections.Generic;
using System.Collections.ObjectModel;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeMid : ITorrentLayer
{
    public TLGroup Group => TLGroup.Eye;
    public string Name => "Mid";
    //public List<Vertex> TorrentPath { get; }
}