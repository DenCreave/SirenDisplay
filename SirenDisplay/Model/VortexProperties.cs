using SirenDisplay.Interfaces;

namespace SirenDisplay.Model;

public class VortexProperties : ILayerProperties
{
    public GPoint[] TorrentPath { get; set; } 
    public double FlowSpeed { get; set; }
    public double DeadzoneRadius { get; set; } // The boundary of our tube
    public double SpringStiffness { get; set; } // How hard the rubber band pulls back (0.1 is smooth, 0.5 is snappy)
    public double MinLateralSpeed { get; set; }
}