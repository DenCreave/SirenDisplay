using System;
using System.Linq;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeBottomTL : ITorrentLayer<VortexProperties>
{
    public TLGroup Group => TLGroup.Eye;
    public TLName Name => TLName.Bottom;
    /*public double? SpawnMinX => 800;
    public double? SpawnMaxX => null;
    public double? SpawnMinY => 100;
    public double? SpawnMaxY => 300;
    public bool? RotateClockwise => null;
    public bool Oscillates => true;
    public double TorrentPower => 80;*/
    public ResNote ResolutionNote => new ResNote() { X = 800, Y = 480, Ratio = "5:3" };
    /*public GPoint[] TorrentPath =>
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
    ]; */
    
    
    public Noise Noise { get; }
    public VortexProperties UniqueProps { get; }
    public AnimatrixController Controls { get; }

    public EyeBottomTL(AnimatrixController controls, DotMapLoader mapLoader)
    {
        Controls = controls;
        var myDotMap = mapLoader.DotMaps[DMGroup.Eye]
            .FirstOrDefault(x => x.TorrentGroup == TLGroup.Eye && x.TorrentLayerType == TLName.Bottom);
    
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
        throw new NotImplementedException();
    }

    public void Despawn()
    {
        throw new NotImplementedException();
    }
}