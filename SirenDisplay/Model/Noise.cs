using SirenDisplay.Controllers;

namespace SirenDisplay.Model;

public sealed class Noise
{
    /// i thought of adding a noise algorithm enum
    /// but i won't use anything other than halton
    public int BaseX { get; init; }
    public int BaseY { get; init; }
    public bool AffectsX { get; init; }
    public bool AffectsY { get; init; }
    public int HaltonDimension { get; init; }
    public int NoiseInstances { get; set; }
    public double NoiseScale { get; init; }
    public double[] HaltonValues1D { get; set; }
}