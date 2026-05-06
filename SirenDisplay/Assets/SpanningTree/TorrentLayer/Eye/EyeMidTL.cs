using System.Collections.Generic;
using System.Collections.ObjectModel;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeMidTL : ITorrentLayer
{
    /* todo: actually, we might not even need the eye as a torrent layer, but a dotmap
     * what im wondering tho, where should i define the desired behaviour?
     * should i define it here? should the torrent layer define not only oscillation but
     * rotation as well? hmmmmmm... actuallllllyyyyyy... yeah... this is the class where
     * the behaviour of the dots should be defined, oscillation or keeping distance,
     * vector based value increase or constant path following
     * ...
     * ill... need... moar... enums!!!
     * PS: DOTMAPS: static, spawn required, always visible, etc...
     * ooooh it will look so good! i hope performant too...
     */
    public TLGroup Group => TLGroup.Eye;
    public TLName Name => TLName.Mid;
    public double? SpawnMinX => null;
    public double? SpawnMaxX => null;
    public double? SpawnMinY => null;
    public double? SpawnMaxY => null;

    public bool? RotateClockwise => false; // let's try other way too
    public bool Oscillates => true;
    public double TorrentPower => 10;
    public ResNote ResolutionNote => new ResNote() { X = 800, Y = 480, Ratio = "5:3" };

    public GPoint[] TorrentPath =>
    [
        new GPoint(){X = 400, Y = 240} //1 pixel to the right but who cares
    ];

    //deleteos
    
    public AnimatrixController Controls { get; }
    public Noise Noise { get; }
    public void AffectVector(Vertex vertex)
    {
        throw new System.NotImplementedException();
    }
}