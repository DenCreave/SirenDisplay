using SirenDisplay.Interfaces;

namespace SirenDisplay.Model.DMProps;

public class IrisProperties : IDMProperties
{
    public double IrisRadiusOffset { get; set; } = 66;
    public double Scale { get; set; } = 5; // shapes are calculated on the origo. scaling needed
    public int ShapeMultiplier { get; set; } = 16; // how many times do we want to produce the same shape.
}