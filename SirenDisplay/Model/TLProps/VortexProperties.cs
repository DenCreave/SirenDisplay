using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Model.TLProps;

public class VortexProperties : ILayerProperties
{
    public Noise Noise { get; set; }
    public GPoint[] TorrentPath { get; set; }
    /*public double SpawnXoffset { get; set; } might be needed later
    public double SpawnYoffset { get; set; }*/
    public double FlowSpeed { get; set; }
    public double DeadzoneRadius { get; set; } // The boundary of our tube
    public double SpringStiffness { get; set; } // How hard the rubber band pulls back (0.1 is smooth, 0.5 is snappy)
    public double MinLateralSpeed { get; set; }
    public Spawner VertexSpawner { get; set; } 
}