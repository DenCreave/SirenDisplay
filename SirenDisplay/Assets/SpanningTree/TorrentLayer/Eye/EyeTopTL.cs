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
        int n = 1000;
        int baseX = 7;
        int baseY = 11;
        UniqueProps = new VortexProperties()
        {
            Noise = new Noise()
            {
                BaseY = 2,
                BaseX = 2,
                AffectsX = false,
                AffectsY = true,
                HaltonDimension = 1,
                NoiseInstances = n,
                NoiseScale = 100, // deadzoneradius*5
                HaltonValuesX = Controls.HaltonSequencer1D(n, baseX),
                HaltonValuesY = Controls.HaltonSequencer1D(n, baseY)
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
            FlowSpeed = 10,
            MinLateralSpeed = 20,
            SpringStiffness = 0.05,
            VertexSpawner = new Spawner()
            {
                LifeTime = 30,
                SpawnInterval = 0.01
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
        vertex.TargetPathIndex = 1; 
        
        // --- THE HASH TRICK (Zero-GC Pseudo-Randomness) ---
        // We multiply the ID by large prime numbers and cast to uint to scramble the bits.
        // This completely destroys the "ribbon" pattern of the Halton sequence.
        uint id = (uint)vertex.ID;
        uint hashX = id * 2654435761u;
        uint hashY = id * 2246822519u;

        int indexX = (int)(hashX % (uint)UniqueProps.Noise.HaltonValuesX.Length);
        int indexY = (int)(hashY % (uint)UniqueProps.Noise.HaltonValuesY.Length);

        // 1. APPLY TRUE 2D NOISE TO COORDINATES
        vertex.Cox = UniqueProps.TorrentPath[0].X + 
                     (UniqueProps.Noise.AffectsX ? (UniqueProps.Noise.HaltonValuesX[indexX] * UniqueProps.Noise.NoiseScale) : 0) 
                     - (UniqueProps.Noise.NoiseScale / 2);
        
        vertex.Coy = UniqueProps.TorrentPath[0].Y + 
                     (UniqueProps.Noise.AffectsY ? (UniqueProps.Noise.HaltonValuesY[indexY] * UniqueProps.Noise.NoiseScale) : 0) 
                     - (UniqueProps.Noise.NoiseScale / 2);

        // 2. CHAOTIC INITIAL MOMENTUM
        double haltonY = UniqueProps.Noise.HaltonValuesY[indexY];
        double haltonX = UniqueProps.Noise.HaltonValuesX[indexX];

        // Even IDs go Right (+1), Odd IDs go Left (-1)
        double direction = (vertex.ID % 2 == 0) ? 1.0 : -1.0;
        
        // Randomize the starting speed using the scrambled Halton values
        double speedMultiplier = 0.5 + (haltonX * 0.5);
        double personalMinSpeed = UniqueProps.MinLateralSpeed * haltonY;
        
        vertex.LateralVector = personalMinSpeed * speedMultiplier * direction;
        
        //// Give it forward legs, but vary the speed! 
        // Transpose Halton (0.0 to 1.0) into a multiplier (0.5 to 1.2)
        double transposeMultiplier = 0.5 + (haltonX * 0.7);
        // Give it forward legs
        vertex.Speed = UniqueProps.FlowSpeed * transposeMultiplier;
        vertex.IsEnabled = true;

    }
    
    public void Despawn(Vertex vertex)
    {
        vertex.IsEnabled = false;
    }
}