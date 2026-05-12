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

    public Path EdgePath { get; set; }

    public PathFigure EdgeFigure { get; set; }
    public LineSegment EndSegment { get; set; }

    public HsvColor StrokeColor { get; set; }
    public HsvColor EffectColor { get; set; }


    //public SolidColorBrush FillColor { get; set; }

    public DropShadowEffect DropShadow { get; set; }

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

    public void InitEdge()
    {//hue is 340 for the vertex, btw the screen has an opacity on 0.8
        StrokeColor = new HsvColor(0.6, 15, 89, 100);
        EffectColor = new HsvColor(0.8, 15, 76, 100);
        
        DropShadow = new DropShadowEffect()
        {
            BlurRadius = 0,
            Color = EffectColor.ToRgb()
        };

    EndSegment = new LineSegment()
        {
            Point = new Point(B.Cox, B.Coy),
        };
        EdgeFigure = new PathFigure()
        {
            StartPoint = new Point(A.Cox, A.Coy),
            Segments = { EndSegment }
        };
        EdgePath = new Path()
        {
            Opacity = Opacity,
            Stroke = new SolidColorBrush(StrokeColor.ToRgb()),
            Effect = DropShadow,
            StrokeThickness = 3,
            Stretch = Stretch.Fill,
            Data = new PathGeometry
            {
                Figures = new PathFigures()
                {
                    EdgeFigure
                }
            }
        };
    }

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
        DropShadow.Opacity = 1;
        DropShadow.BlurRadius = 0;
        IsAnimating = true;
        Enimation = true;

        return this;
    }
    
    public VexEdge EnimateMe()//iThInKiTsCuTe<3
    {
        if (DropShadow.BlurRadius>49)//thats basically a 50 frames lock
        {
            ++DropShadow.BlurRadius;
        }
        else
        {
            DropShadow.Opacity = 0;
            Enimation = false;
            IsAnimating = false;
        }

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

    public VexEdge UpdateEdgeCO()
    {
        EdgeFigure.StartPoint = new Point(A.Cox, A.Coy);
        EndSegment.Point = new Point(B.Cox, B.Coy);
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