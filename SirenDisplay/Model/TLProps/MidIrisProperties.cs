using System;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Model.TLProps;

public class MidIrisProperties : ILayerProperties
{
    // private double _degreesPerFrame { get; set; } // visible for me
    public double RadiansPerFrame { get; init; } // calculated in the constructor

    public MidIrisProperties(double degreesPerFrame)
    {
        RadiansPerFrame = degreesPerFrame * (Math.PI / 180.0);
    }
}