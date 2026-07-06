using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.TLProps;
using SirenDisplay.SpanningTree.Controller;
using SirenDisplay.SpanningTree.Theme;

namespace SirenDisplay.SpanningTree.TorrentLayer.Eye;

public sealed class EyeBottomTL : ITorrentLayer<VortexProperties>
{
    public ThemeGroup Group => ThemeGroup.Eye;
    public TLName Name => TLName.Bottom;
    public ResNote ResolutionNote => new ResNote() { X = 800, Y = 480, Ratio = "5:3" };

    public VortexProperties UniqueProps { get; }
    public AnimatrixController Controls { get; }
    public RenderAlignment Align { get; }


    // well it has to be animated even after lifetime ends
    // afterall spawner just stops spawning, and it disappears once
    // it reaches the end of its path.
    public bool IsAffecting { get; }

    public bool IsVisible { get; }

    public EyeBottomTL(AnimatrixController controls)
    {
        IsVisible = true;
        IsAffecting = true;
        Controls = controls;
        int n = 1000;
        int baseX = 7;
        int baseY = 11;
        UniqueProps = new VortexProperties()
        {
            Noise = new Noise()
            {
                BaseX = 2,
                BaseY = 3,
                AffectsX = true,
                AffectsY = true,
                HaltonDimension = 2,
                NoiseInstances = n,
                NoiseScale = 100, // deadzoneradius*5
                HaltonValuesX = Controls.HaltonSequencer1D(n, baseX),
                HaltonValuesY = Controls.HaltonSequencer1D(n, baseY)
            },
            TorrentPath =
            [
                new GPoint() { X = 800, Y = 100 },
                new GPoint() { X = 765, Y = 125 },
                new GPoint() { X = 730, Y = 165 },
                new GPoint() { X = 705, Y = 205 },
                new GPoint() { X = 680, Y = 245 },
                new GPoint() { X = 640, Y = 285 },
                new GPoint() { X = 600, Y = 325 },
                new GPoint() { X = 560, Y = 345 },
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


        /*// 1. Get the shared index
        int index = vertex.ID % UniqueProps.Noise.HaltonValuesX.Length;
        // 2. APPLY TRUE 2D NOISE TO COORDINATES
        vertex.Cox = UniqueProps.TorrentPath[0].X +
                     (UniqueProps.Noise.AffectsX ? (UniqueProps.Noise.HaltonValuesX[index] * UniqueProps.Noise.NoiseScale) : 0)
                     - (UniqueProps.Noise.NoiseScale / 2);

        vertex.Coy = UniqueProps.TorrentPath[0].Y +
                     (UniqueProps.Noise.AffectsY ? (UniqueProps.Noise.HaltonValuesY[index] * UniqueProps.Noise.NoiseScale) : 0)
                     - (UniqueProps.Noise.NoiseScale / 2);

        vertex.IsEnabled = true;
        vertex.TargetPathIndex = 1;
        vertex.Speed = UniqueProps.FlowSpeed;

        // Use the X Halton value to vary the starting speed slightly
        double speedInit = 0.5 + (UniqueProps.Noise.HaltonValuesX[index] * 0.5);


        // 2. Even IDs go Right (+1), Odd IDs go Left (-1)
        double direction = (vertex.ID % 2 == 0) ? 1.0 : -1.0;

        // 3. Combine them! (e.g., 0.8 * 20 * -1 = -16.0)
        vertex.LateralVector = speedInit * UniqueProps.MinLateralSpeed * direction;
*/
    }

    public void Despawn(Vertex vertex)
    {
        vertex.IsEnabled = false;
    }
}