using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Model;

namespace SirenDisplay.Interfaces;
/// <summary>
/// defines the behaviour of a dotmap
/// </summary>
public interface ITorrentLayer
{
    TLGroup Group { get; }
    TLName Name { get; } // dotmaps use the group then this to locate the TL
    double? SpawnMinX { get; }
    double? SpawnMaxX { get; }
    double? SpawnMinY { get; }
    double? SpawnMaxY { get; }
    bool? RotateClockwise { get; } //null: doesnt rotate at all therefore follows path
    bool Oscillates { get; } //oscillates adjacent to a line between two points
    double TorrentPower { get; } //speed; 
    ResNote ResolutionNote { get; } 
    GPoint[] TorrentPath { get; } 
    
    
    //todo, maybe add a layer order too
}