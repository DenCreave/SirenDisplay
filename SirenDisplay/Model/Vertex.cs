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
    private double _x;
    [ObservableProperty]
    private double _y;


    public double Cox {get; set;}
    public double Coy {get; set;}
    public double Vex { get; set; }
    public double Vey { get; set; }
    public double Speed {get; set;}
    public double Weight { get; set; } = 1;
    public int EdgeLimit { get; set; } //to run a modified kruskal, wonder how it'll look
    public bool Equals(Vertex? other)
    {//.contains checks if the a vertex exists with the same coordinates
        return Cox == other.Cox && Coy == other.Coy;
    }
}