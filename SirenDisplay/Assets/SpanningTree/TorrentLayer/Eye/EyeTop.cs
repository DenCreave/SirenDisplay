using System.Collections.Generic;
using System.Collections.ObjectModel;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeTop : ITorrentLayer
{
    public TLGroup Group => TLGroup.Eye;
    public string Name => "Top";
    public double? SpawnMinX => 0;
    public double? SpawnMaxX => null;
    public double? SpawnMinY => 200;
    public double? SpawnMaxY => 380;
    public ResNote ResolutionNote => new ResNote(){X = 800, Y = 480, Ratio = "5:3"};
    public GPoint[] TorrentPath => 
    [
        new GPoint(){X = 0,Y = 380},
        new GPoint(){X = 45,Y = 335},
        new GPoint(){X = 80,Y = 290},
        new GPoint(){X = 110,Y = 260},
        new GPoint(){X = 145,Y = 220},
        new GPoint(){X = 185,Y = 170},
        new GPoint(){X = 230,Y = 140},
        new GPoint(){X = 300,Y = 110},
        new GPoint(){X = 380,Y = 95},
        new GPoint(){X = 465,Y = 95},
        new GPoint(){X = 525,Y = 110},
        new GPoint(){X = 585,Y = 135},
        new GPoint(){X = 645,Y = 145},
        new GPoint(){X = 705,Y = 135},
        new GPoint(){X = 755,Y = 115},
        new GPoint(){X = 800,Y = 80}
    ];
    
    /*
     public GPoint[] TorrentPath => 
       [
           new GPoint(){X = 0,Y = 100},
           new GPoint(){X = 45,Y = 145},
           new GPoint(){X = 80,Y = 190},
           new GPoint(){X = 110,Y = 220},
           new GPoint(){X = 145,Y = 260},
           new GPoint(){X = 185,Y = 310},
           new GPoint(){X = 230,Y = 340},
           new GPoint(){X = 300,Y = 370},
           new GPoint(){X = 380,Y = 385},
           new GPoint(){X = 465,Y = 385},
           new GPoint(){X = 525,Y = 370},
           new GPoint(){X = 585,Y = 345},
           new GPoint(){X = 645,Y = 335},
           new GPoint(){X = 705,Y = 345},
           new GPoint(){X = 755,Y = 365},
           new GPoint(){X = 800,Y = 400}
       ];
     */
}