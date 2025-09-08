using System.Collections.Generic;
using SirenDisplay.Model;

namespace SirenDisplay.Interfaces;

public interface ITorrentLayer
{
    TLGroup Group { get; }
    string Name { get; }
    double? SpawnMinX { get; }
    double? SpawnMaxX { get; }
    double? SpawnMinY { get; }
    double? SpawnMaxY { get; }
    ResNote ResolutionNote { get; } 
    GPoint[] TorrentPath { get; } 
    
    
    //todo, maybe add a layer order too
}