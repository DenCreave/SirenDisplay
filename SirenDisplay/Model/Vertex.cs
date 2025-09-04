using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http.Headers;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SirenDisplay.Model;

public sealed partial class Vertex : ObservableObject, IEquatable<Vertex>
{ 
    [ObservableProperty]
    private int _x;
    [ObservableProperty]
    private int _y;


    public int Cox {get; set;}
    public int Coy {get; set;}
    public int Vex { get; set; }
    public int Vey { get; set; }
    public double Speed {get; set;}
    public double Weight { get; set; }
    public int EdgeLimit { get; set; } //to run a modified kruskal, wonder how it'll look
    public bool Equals(Vertex? other)
    {//.contains checks if the a vertex exists with the same coordinates
        return Cox == other.Cox && Coy == other.Coy;
    }
}