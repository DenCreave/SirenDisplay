using System.Collections.Generic;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer;

public class testg : ITorrentLayer
{
    private GPoint[] _torrentPath;
    public TLGroup Group => TLGroup.Test;
    public string Name => "test3";
    public List<GPoint> TorrentPath { get; }

    GPoint[] ITorrentLayer.TorrentPath => _torrentPath;

    public double? SpawnMinX { get; }
    public double? SpawnMaxX { get; }
    public double? SpawnMinY { get; }
    public double? SpawnMaxY { get; }
    public ResNote ResolutionNote { get; }
}