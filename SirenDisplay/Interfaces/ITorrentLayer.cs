using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.Theme;
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
    ThemeGroup Group { get; }
    TLName Name { get; } // dotmaps use the group then this to locate the TL
    ResNote ResolutionNote { get; }
    AnimatrixController Controls { get; }
    RenderAlignment Align { get; }
    public void Init();
    public void Reset();
    public void Spawn(Vertex vertex);
    public void Despawn(Vertex vertex);
    public void AffectVector(Vertex vertex);
    //todo, maybe add a layer order too
}
public interface ITorrentLayer<T> : ITorrentLayer 
    where T : ILayerProperties
{
    T UniqueProps { get; }
}

