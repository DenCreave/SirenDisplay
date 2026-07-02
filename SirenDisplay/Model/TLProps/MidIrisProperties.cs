using System;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Model.TLProps;

public class MidIrisProperties : ILayerProperties
{
    // private double _degreesPerFrame { get; set; } // visible for me
    public double RadiansPerFrame { get; init; } // calculated in the constructor

    public Fader FaderConf { get; set; }

    public MidIrisProperties(double degreesPerFrame)
    {
        RadiansPerFrame = degreesPerFrame * (Math.PI / 180.0);
    }
}