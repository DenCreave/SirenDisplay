using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http.Headers;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SirenDisplay.Model;

public sealed partial class Vertex : ObservableObject, IComparable<Vertex>, IEquatable<Vertex>
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

    public Vertex CMPRoot { get; set; }
    public double CMPRootDistance { get; set; }

    public void UpdateDistance()
    {
        CMPRootDistance = Math.Sqrt(Math.Pow(Cox - CMPRoot.Cox, 2) + Math.Pow(Coy - CMPRoot.Coy, 2))*Weight*CMPRoot.Weight;
    }

    public int CompareTo(Vertex? other)
    {
        /*return (Math.Sqrt(Math.Pow(Cox - CMPRoot.Cox, 2) + Math.Pow(Coy - CMPRoot.Coy, 2))*Weight*CMPRoot.Weight)
            .CompareTo((Math.Sqrt(Math.Pow(other.Cox - other.CMPRoot.Cox, 2) + Math.Pow(other.Coy - other.CMPRoot.Coy, 2))*other.Weight*CMPRoot.Weight)) ;*/
        return CMPRootDistance.CompareTo(other?.CMPRootDistance);
    }

    public bool Equals(Vertex? other)
    {
        return Cox == other.Cox && Coy == other.Coy;
    }
}