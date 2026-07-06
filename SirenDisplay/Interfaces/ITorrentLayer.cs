using SirenDisplay.Controllers;
using SirenDisplay.Model;
using SirenDisplay.SpanningTree.Theme;
using SirenDisplay.SpanningTree.TorrentLayer;

namespace SirenDisplay.Interfaces;
/// <summary>
/// defines the behaviour of a dotmap
/// </summary>
public interface ITorrentLayer
{
    ThemeGroup Group { get; }
    TLName Name { get; } // dotmaps use the group then this to locate the TL
    ResNote ResolutionNote { get; }
    AnimatrixController Controls { get; }
    RenderAlignment Align { get; }
    public bool IsAffecting { get; }
    public bool IsVisible { get; } // for the ui
    public void Reset();
    public void UpdateState(double deltaTime);
    public void Spawn(Vertex vertex);
    public void Despawn(Vertex vertex);
    public void AffectVector(Vertex vertex);
}
public interface ITorrentLayer<T> : ITorrentLayer 
    where T : ILayerProperties
{
    T UniqueProps { get; }
}

