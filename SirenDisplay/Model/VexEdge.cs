using System;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace SirenDisplay.Model;

public sealed class VexEdge : IEquatable<VexEdge>, IComparable<VexEdge>
{
    public Vertex A { get; set; }
    public Vertex B { get; set; }
    public double Opacity { get; set; }
    public EdgeRelType RelationType { get; set; }
    
    // group
    public Insignia Group { get; set; }
    public double Distance { get; set; }

    //ive decided that animating this object will be handled via
    //a controller in loop, so im making flag for that controller
    //to know what to do with this object each iteration
    public bool IsEnabled { get; set; } = false;
    //actually, ive decided why IsAnimating is important,
    //it would be quite fckin annoying causing a seizure if the enable
    //animation would get triggered every second fcking frame
    //so im giving it a flag, so even if its not in my custom krskl
    //it wont bounce in and out causing an epilepsy shock from a single edge
    public bool IsAnimating { get; set; } = false; 
    public bool Enimation { get; set; } = false; //no, its not
    public bool Disamation { get; set; } = false; //naming cancer

    public VexEdge UpdateDistance()
    {
        Distance = Math.Sqrt(Math.Pow(A.Cox - B.Cox, 2) + Math.Pow(A.Coy - B.Coy, 2)) * A.Weight * B.Weight;
        return this;
    }

    ///todo somehow make control these animations... i think ive got an idea
    /// maybe that tick thingie like with the clock, although not sure
    /// DispatcherTimer
    public VexEdge EnableAnimation()
    {
        IsEnabled = true;
        Opacity = 1;
        IsAnimating = true;
        Enimation = true;

        return this;
    }
    
    public VexEdge DisableAnimation()
    {
        IsAnimating = true;
        Disamation = true;
        return this;
    }

    public VexEdge DisamateMe()
    {
        if (Opacity > 0)
        {
            Opacity = Double.Round(Opacity - 0.1,1);
        }
        else
        {
            IsEnabled = false;
            IsAnimating = false;
            Disamation = false;
        }
        return this;
    }

    public bool Equals(VexEdge? other)
    {
        // .contains check if the point is already added (checks based of reference not value)
        return (this.A == other.A && this.B == other.B) || (this.A == other.B && this.B == other.A);
    }

    public int CompareTo(VexEdge? other)
    {
        return Distance.CompareTo(other?.Distance);
    }
}