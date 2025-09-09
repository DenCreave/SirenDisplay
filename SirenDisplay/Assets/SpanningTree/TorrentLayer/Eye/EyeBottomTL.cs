using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeBottomTL : ITorrentLayer
{
    public TLGroup Group => TLGroup.Eye;
    public TLName Name => TLName.Bottom;
    public double? SpawnMinX => 800;
    public double? SpawnMaxX => null;
    public double? SpawnMinY => 100;
    public double? SpawnMaxY => 300;
    public bool? RotateClockwise => null;
    public bool Oscillates => true;
    public double TorrentPower => 80;
    public ResNote ResolutionNote => new ResNote() { X = 800, Y = 480, Ratio = "5:3" };
    public GPoint[] TorrentPath =>
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
    ]; 
    /*
     public GPoint[] TorrentPath =>
       [
           new GPoint() { X = 800, Y = 400 },
           new GPoint() { X = 765, Y = 360 },
           new GPoint() { X = 730, Y = 315 },
           new GPoint() { X = 705, Y = 275 },
           new GPoint() { X = 680, Y = 235 },
           new GPoint() { X = 640, Y = 195 },
           new GPoint() { X = 600, Y = 155 },
           new GPoint() { X = 560, Y = 110 },
           new GPoint() { X = 515, Y = 80 },
           new GPoint() { X = 470, Y = 55 },
           new GPoint() { X = 405, Y = 45 },
           new GPoint() { X = 340, Y = 45 },
           new GPoint() { X = 290, Y = 55 },
           new GPoint() { X = 245, Y = 70 },
           new GPoint() { X = 200, Y = 90 },
           new GPoint() { X = 145, Y = 100 },
           new GPoint() { X = 95, Y = 105 },
           new GPoint() { X = 40, Y = 95 },
           new GPoint() { X = 0, Y = 80 },
       ];
     
     */
}