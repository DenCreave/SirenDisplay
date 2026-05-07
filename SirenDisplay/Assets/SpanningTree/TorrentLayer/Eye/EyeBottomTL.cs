using System;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.TLProps;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeBottomTL : ITorrentLayer<VortexProperties>
{
    public ThemeGroup Group => ThemeGroup.Eye;
    public TLName Name => TLName.Bottom;
    public ResNote ResolutionNote => new ResNote() { X = 800, Y = 480, Ratio = "5:3" };

    public VortexProperties UniqueProps { get; }
    public AnimatrixController Controls { get; }

    public EyeBottomTL(AnimatrixController controls, DotMapLoader mapLoader)
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
                new GPoint() { X = 800, Y = 80 },
                new GPoint() { X = 765, Y = 120 },
                new GPoint() { X = 730, Y = 165 },
                new GPoint() { X = 705, Y = 205 },
                new GPoint() { X = 680, Y = 245 },
                new GPoint() { X = 640, Y = 285 },
                new GPoint() { X = 600, Y = 325 },
                new GPoint() { X = 560, Y = 110 },
                new GPoint() { X = 515, Y = 370 },
                new GPoint() { X = 470, Y = 425 },
                new GPoint() { X = 405, Y = 435 },
                new GPoint() { X = 340, Y = 435 },
                new GPoint() { X = 290, Y = 425 },
                new GPoint() { X = 245, Y = 410 },
                new GPoint() { X = 200, Y = 390 },
                new GPoint() { X = 145, Y = 380 },
                new GPoint() { X = 95, Y = 375 },
                new GPoint() { X = 40, Y = 385 },
                new GPoint() { X = 0, Y = 400 },
            ],
            DeadzoneRadius = 20,
            FlowSpeed = 20,
            MinLateralSpeed = 20,
            SpringStiffness = 0.05,
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