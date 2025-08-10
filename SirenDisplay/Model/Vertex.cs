using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http.Headers;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SirenDisplay.Model;

public sealed partial class Vertex : ObservableObject
{ 
    [ObservableProperty]
    private int _x;
    [ObservableProperty]
    private int _y;
    
    public int Cox {get; set;}
    public int Coy {get; set;}
    public int Vex { get; set; }
    public int Vey { get; set; }
    public double Weight { get; set; }
    
    public List<Vertex> DistanceVector = new();
    public List<LineSegment> Edges = new();
}