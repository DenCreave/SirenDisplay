using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeMidTL : ITorrentLayer
{
    public ThemeGroup Group => ThemeGroup.Eye;
    public TLName Name => TLName.Mid;
    /*public double? SpawnMinX => null;
    public double? SpawnMaxX => null;
    public double? SpawnMinY => null;
    public double? SpawnMaxY => null;

    public bool? RotateClockwise => false; // let's try other way too
    public bool Oscillates => true;
    public double TorrentPower => 10;*/
    public ResNote ResolutionNote => new ResNote() { X = 800, Y = 480, Ratio = "5:3" };

    /*public GPoint[] TorrentPath =>
    [
        new GPoint(){X = 400, Y = 240} //1 pixel to the right but who cares
    ];*/
    
    public AnimatrixController Controls { get; }

    public void Init()
    {
        throw new System.NotImplementedException();
    }

    public void Spawn(Vertex vertex)
    {
        throw new System.NotImplementedException();
    }

    public void Despawn(Vertex vertex)
    {
        throw new System.NotImplementedException();
    }

    public void AffectVector(Vertex vertex)
    {
        throw new System.NotImplementedException();
    }
}