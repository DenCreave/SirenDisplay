using System;
using System.Linq;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.TLProps;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeTopTL : ITorrentLayer<VortexProperties>
{
    public ThemeGroup Group { get; } = ThemeGroup.Eye;
    public TLName Name { get; } = TLName.Top;
    public ResNote ResolutionNote { get; } = new ResNote() { X = 800, Y = 480, Ratio = "5:3" };
    public VortexProperties UniqueProps { get; }
    public AnimatrixController Controls { get; }
    public RenderAlignment Align { get; }
    public bool IsAffecting { get; }
    public bool IsVisible { get; }

    public EyeTopTL(AnimatrixController controls)
    {
        Controls = controls;
        IsAffecting = true;
        IsVisible = true;
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
            SpringStiffness = 0.05,
            VertexSpawner = new Spawner()
            {
                LifeTime = 30,
                SpawnInterval = 0.06
            }
        };

        Align = RenderAlignment.Unchanged;
    }
    
    public void UpdateState(double deltaTime)
    {
        UniqueProps.VertexSpawner.UpdateTime(deltaTime);
    }

    public void AffectVector(Vertex vertex)
    {
        if (!vertex.IsEnabled)
        {
            if (UniqueProps.VertexSpawner.TryConsumeSpawn())
            {
                Spawn(vertex);
            }
            return;
        }
        Controls.Vortexer(UniqueProps, vertex);
        
        
        if (vertex.TargetPathIndex > UniqueProps.TorrentPath.Length - 1)
        {
            Despawn(vertex);
        }
    }
    
    public void Reset()
    {
        
        UniqueProps.VertexSpawner.Reset();
    }

    public void Spawn(Vertex vertex)
    {
        vertex.Cox = UniqueProps.TorrentPath[0].X + (UniqueProps.Noise.NoiseScale * 
                                                     (UniqueProps.Noise.AffectsX 
                                                         ? UniqueProps.Noise.HaltonValues1D[vertex.ID%UniqueProps.Noise.HaltonValues1D.Length] : 0))
                     -UniqueProps.Noise.NoiseScale/2;
        
        vertex.Coy = UniqueProps.TorrentPath[0].Y  + (UniqueProps.Noise.NoiseScale * 
                                                      (UniqueProps.Noise.AffectsY
                                                          ? UniqueProps.Noise.HaltonValues1D[vertex.ID%UniqueProps.Noise.HaltonValues1D.Length] : 0))
                     -UniqueProps.Noise.NoiseScale/2;
        vertex.IsEnabled = true;
    }
    
    public void Despawn(Vertex vertex)
    {
        vertex.IsEnabled = false;
    }
}