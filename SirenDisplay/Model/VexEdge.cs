using System;

namespace SirenDisplay.Model;

public sealed class VexEdge : IEquatable<VexEdge>, IComparable<VexEdge>
{
    public Vertex A {get; set;}
    public Vertex B {get; set;}
    //Linesegment
    //opacity
    //color
    //maybe an animation idk.
    public double Distance {get; set;}
    public bool IsEnabled { get; set; } = false;

    public VexEdge UpdateDistance()
    {
        Distance = Math.Sqrt(Math.Pow(A.Cox - B.Cox, 2) + Math.Pow(A.Coy - B.Coy, 2)) * A.Weight * B.Weight;
        return this;
    }

    public bool Equals(VexEdge? other)
    { // .contains check if the point is already added (checks based of reference not value)
        return (this.A == other.A && this.B == other.B) || (this.A == other.B && this.B == other.A );
    }

    public int CompareTo(VexEdge? other)
    {
        return Distance.CompareTo(other?.Distance);
    }
}