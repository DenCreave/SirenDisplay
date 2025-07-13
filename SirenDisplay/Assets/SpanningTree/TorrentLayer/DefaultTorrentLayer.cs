using System.Collections.ObjectModel;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer;

public sealed class DefaultTorrentLayer : ITorrentLayer
{
    public string Name => "Default";
    public ObservableCollection<Vertex> Vertices { get; set; }
    public void IncreaseVertices()
    {
        throw new System.NotImplementedException();
    }

    public void DecreaseVertices()
    {
        throw new System.NotImplementedException();
    }
}