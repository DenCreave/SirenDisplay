using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Controllers;
using SirenDisplay.Model;

namespace SirenDisplay.Interfaces;
/// <summary>
/// defines the behaviour of a dotmap
/// </summary>
public interface ITorrentLayer
{
    ///todo add a Halton sequence for the guiding path
    /// so that way each point could have a slightly noisy path
    TLGroup Group { get; }
    TLName Name { get; } // dotmaps use the group then this to locate the TL

    /*double? SpawnMinX { get; }
    double? SpawnMaxX { get; }
    double? SpawnMinY { get; }
    double? SpawnMaxY { get; }
    bool? RotateClockwise { get; } //null: doesnt rotate at all therefore follows path
    bool Oscillates { get; } //oscillates adjacent to a line between two points
    double TorrentPower { get; } //speed;
    GPoint[] TorrentPath { get; }*/
    ResNote ResolutionNote { get; }
    AnimatrixController Controls { get; }
    Noise Noise { get; }
    public void Init();
    public void Spawn(Vertex vertex);
    public void Despawn(Vertex vertex);
    //spawn and despawn handled by the meshheap handler based on the torrentlayer
    public void AffectVector(Vertex vertex);
    //todo, maybe add a layer order too
}
public interface ITorrentLayer<T> : ITorrentLayer 
    where T : ILayerProperties
{
    T UniqueProps { get; }
}

