using SirenDisplay.Interfaces;

namespace SirenDisplay.Model.TLProps;

public class MidIrisProperties : ILayerProperties
{
    // setting default values as well, default res 800x480
    public double OffsetX { get; set; } = 400;
    public double OffsetY { get; set; } = 240;
    public double RotationSpeed { get; set; } = 0.05;
}