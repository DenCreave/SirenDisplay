using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeTopTL : ITorrentLayer<VortexProperties>
{
    public TLGroup Group => TLGroup.Eye;
    public TLName Name => TLName.Top;
    /*public double? SpawnMinX { get; } = 0;
    public double? SpawnMaxX { get; } = null;
    public double? SpawnMinY { get; } = 200;
    public double? SpawnMaxY { get; } = 380;
    public bool? RotateClockwise { get; } = null;
    public bool Oscillates { get; } = true;
    public double TorrentPower { get; } = 80;*/
 
    public ResNote ResolutionNote => new ResNote() { X = 800, Y = 480, Ratio = "5:3" };

    /*public GPoint[] TorrentPath { get; } =
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
    ];*/

    public Noise Noise { get; }
    public VortexProperties UniqueProps { get; }
    public AnimatrixController Controls { get; }


    public EyeTopTL(AnimatrixController controls, DotMapLoader mapLoader)
    {
        Controls = controls;
        var myDotMap = mapLoader.DotMaps[DMGroup.Eye]
            .FirstOrDefault(x => x.TorrentGroup == TLGroup.Eye && x.TorrentLayerType == TLName.Top);

        Noise = new Noise()
        {
            BaseY = 2,
            BaseX = 2,
            AffectsX = false,
            AffectsY = true,
            HaltonDimension = 1,
            NoiseInstances = myDotMap.DotLimit ?? 0,
            NoiseScale = 20,
            HaltonValues1D = Controls.HaltonSequencer1D(myDotMap.DotLimit ?? 0, 2)
        };

        UniqueProps = new VortexProperties()
        {
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
        Controls.Vortexer(UniqueProps, vertex);
    }

    public void Init()
    {
        throw new NotImplementedException();
    }

    public void Spawn(Vertex vertex)
    {
        // todo the respawn and despawn logic should come here.
        vertex.Cox = UniqueProps.TorrentPath[0].X + (Noise.AffectsX 
            ? Noise.HaltonValues1D[vertex.HaltonIndex] : 0);
        vertex.Coy = UniqueProps.TorrentPath[0].Y  + (Noise.AffectsY
            ? Noise.HaltonValues1D[vertex.HaltonIndex] : 0);
        vertex.IsEnabled = true;
    }

    

    
    public void Despawn(Vertex vertex)
    {
        // Loop back to the start if we reached the end of the whole path
        if (vertex.TargetPathIndex > UniqueProps.TorrentPath.Length - 1)
        {
            // the reason index is 1: A-B line, and we are going towards B
            vertex.TargetPathIndex = 1;
            vertex.IsEnabled = false;
        }
    }
}