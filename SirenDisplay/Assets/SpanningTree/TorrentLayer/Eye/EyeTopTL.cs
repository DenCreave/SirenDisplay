using System;
using System.Linq;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.TLProps;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeTopTL : ITorrentLayer<VortexProperties>
{
    public ThemeGroup Group => ThemeGroup.Eye;
    public TLName Name => TLName.Top;
    public ResNote ResolutionNote => new ResNote() { X = 800, Y = 480, Ratio = "5:3" };
    public VortexProperties UniqueProps { get; }
    public AnimatrixController Controls { get; }


    public EyeTopTL(AnimatrixController controls)
    {
        Controls = controls;

        UniqueProps = new VortexProperties()
        {
            Noise = new Noise()
            {
                BaseY = 2,
                BaseX = 2,
                AffectsX = false,
                AffectsY = true,
                HaltonDimension = 1,
                NoiseInstances = 50,
                NoiseScale = 40, // deadzoneradius*2
                HaltonValues1D = Controls.HaltonSequencer1D(50, 2)
            },
            TorrentPath =
            [
                new GPoint() { X = 0, Y = 380 },
                new GPoint() { X = 45, Y = 335 },
                new GPoint() { X = 80, Y = 290 },
                new GPoint() { X = 110, Y = 260 },
                new GPoint() { X = 145, Y = 220 },
                new GPoint() { X = 185, Y = 170 },
                new GPoint() { X = 230, Y = 140 },
                new GPoint() { X = 300, Y = 110 },
                new GPoint() { X = 380, Y = 95 },
                new GPoint() { X = 465, Y = 95 },
                new GPoint() { X = 525, Y = 110 },
                new GPoint() { X = 585, Y = 135 },
                new GPoint() { X = 645, Y = 145 },
                new GPoint() { X = 705, Y = 135 },
                new GPoint() { X = 755, Y = 115 },
                new GPoint() { X = 800, Y = 80 }
            ],
            DeadzoneRadius = 20,
            FlowSpeed = 20,
            MinLateralSpeed = 20,
            SpringStiffness = 0.05
        };


    }

    public void AffectVector(Vertex vertex)
    {
        ///todo: will add a timelimit for how long these can be visible
        /// note: self tick limit so that i can control the flow of
        /// particles for a given time or amount
        if (!vertex.IsEnabled)
        {
            Spawn(vertex);
            return;
        }
        Controls.Vortexer(UniqueProps, vertex);
        
        ///todo: these despawns will be handled by a dedicated UI handler
        /// which can be inherited from the Control class in avaloniaUI
        if (vertex.TargetPathIndex > UniqueProps.TorrentPath.Length - 1)
        {
            Despawn(vertex);
        }
    }

    public void Init()
    {
        throw new NotImplementedException();
    }

    public void Spawn(Vertex vertex)
    {
        ///todo iteration is needed for halton sequence.
        /// it will have to be concurrent safe.
        
        vertex.Cox = UniqueProps.TorrentPath[0].X + (UniqueProps.Noise.NoiseScale * 
                                                     (UniqueProps.Noise.AffectsX 
                                                         ? UniqueProps.Noise.HaltonValues1D[vertex.ID&UniqueProps.Noise.HaltonValues1D.Length] : 0))
                     -UniqueProps.Noise.NoiseScale/2;
        
        vertex.Coy = UniqueProps.TorrentPath[0].Y  + (UniqueProps.Noise.NoiseScale * 
                                                      (UniqueProps.Noise.AffectsY
                                                          ? UniqueProps.Noise.HaltonValues1D[vertex.ID&UniqueProps.Noise.HaltonValues1D.Length] : 0))
                     -UniqueProps.Noise.NoiseScale/2;
        vertex.IsEnabled = true;
    }
    
    public void Despawn(Vertex vertex)
    {
        vertex.IsEnabled = false;
    }
}