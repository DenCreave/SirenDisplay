using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http.Headers;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SirenDisplay.Model;

public sealed class Vertex : IEquatable<Vertex>
{
    public int TargetPathIndex { get; set; } = 1; // Remembers which point it's heading to
    public int HaltonIndex { get; set; }     // Assigned at spawn so noise is permanent
    public int Ticks { get; set; } = 0;
    public double Cox {get; set;}
    public double Coy {get; set;}
    
    public double Vex {get; set;} // absolute vector x
    public double Vey {get; set;} // absolute vector y
    
    public double FlowVector { get; set; } // relative vector forward
    public double LateralVector { get; set; } // relative vector sideways
    
    public double VectorWeight { get; set; } = 1;
    
    // Private fields to hold the final calculated movement before we apply it
    private double _finalMovementStepX = 0;
    private double _finalMovementStepY = 0;

    
    public double Speed { get; set; } = 1; //multiplier of speed 
    public double Weight { get; set; } = 1; //multiplier in edges
    public int? EdgeLimit { get; set; } = null; //to run a modified kruskal, wonder how it'll look; null: no limits
    public bool IsEnabled { get; set; } = false;
    
    
    public HsvColor FillColor { get; set; }
    public HsvColor EffectColor { get; set; }
    public Path VertexPath {get; set;}
    public PathFigure Crest { get; set; }
    public ArcSegment ArcA {get; set;}
    public ArcSegment ArcB {get; set;}
    public readonly double Diameter = 7;
    public readonly double Radius = 3.5;
    public readonly double fillOpacity = 0.6;
    public readonly double effectOpacity = 0.8;
    public DropShadowEffect DropShadow {get; set;}

    public Vertex InitVertex()
    {
        FillColor = new HsvColor(fillOpacity, 340, 89, 100);
        EffectColor = new HsvColor(effectOpacity, 340, 76, 100);
        ArcA = new ArcSegment()
        {
            Point = new Point(Cox + Diameter, Coy),
            Size = new Size(Radius, Radius),
            SweepDirection = SweepDirection.Clockwise,
            IsLargeArc = false
        };
        ArcB = new ArcSegment()
        {
            Point = new Point(Cox - Radius, Coy),
            Size = new Size(Radius, Radius),
            SweepDirection = SweepDirection.Clockwise,
            IsLargeArc = false
        };
        Crest = new PathFigure()
        {
            StartPoint = new Point(Cox, Coy),
            Segments = { ArcA, ArcB }
        };
        DropShadow = new DropShadowEffect()
        {
            BlurRadius = 21,
            OffsetX = Radius,
            Opacity = effectOpacity,
            Color = EffectColor.ToRgb(),
        };
        VertexPath = new Path()
        {
            Stroke = new SolidColorBrush(EffectColor.ToRgb()),
            StrokeThickness = 1,
            Stretch = Stretch.Uniform,
            Fill = new SolidColorBrush(FillColor.ToRgb()),
            Effect = DropShadow,
            Data = new PathGeometry()
            {
                Figures = new PathFigures() { Crest }
            }
        };

        return this;
    }
    
    //TODO UPDATE THE POINTS EVERYWHERE
    public Vertex UpdateCO()
    {
        /*// 1. Calculate the exact mathematical step based on our local axes
        CalculateVectorDirection();

        // 2. Apply the calculated step to our actual coordinates, 
        // multiplied by the overall Speed modifier.
        Cox += _finalMovementStepX * Speed;
        Coy += _finalMovementStepY * Speed;*/
        
        //keeping coordinates within bounds of the screen will be the task of the handlers
        //todo test this formula
        Cox+=Vex*Speed;
        Coy+=Vey*Speed;
        return this;
    }

    public Vertex UpdateUI()
    {
        if (Crest != null && ArcA != null && ArcB != null)
        {
            Crest.StartPoint = new Point(Cox, Coy);
            ArcA.Point = new Point(Cox + Diameter, Coy);
            ArcB.Point = new Point(Cox - Radius, Coy);
        }
    
        return this;
    }
    
    /// <summary>
    /// Calculates a normalized direction vector based on proportional forward and sideways weights.
    /// </summary>
    /// <param name="targetX">The X coordinate of the point we want to flow towards.</param>
    /// <param name="targetY">The Y coordinate of the point we want to flow towards.</param>
    /// <param name="flow">The weight of the forward pull (e.g., 10).</param>
    /// <param name="lateral">The weight of the sideways pull (e.g., 5). Positive is right, Negative is left.</param>
    public void CalculateVectorDirection(double targetX, double targetY, double flow, double lateral)
    {
        // STEP 1: Find the raw distances between our current position and the target
        double distanceX = targetX - Cox;
        double distanceY = targetY - Coy;

        // STEP 2: Calculate the straight-line distance to the target
        double distanceToTarget = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));

        if (distanceToTarget > 0) 
        {
            // STEP 3: NORMALIZE THE FORWARD DIRECTION
            // This gives us a pure direction pointing at the target, with a length of exactly 1.
            double normalizedForwardDirX = distanceX / distanceToTarget;
            double normalizedForwardDirY = distanceY / distanceToTarget;

            // STEP 4: CALCULATE THE NORMALIZED LATERAL (SIDEWAYS) DIRECTION
            // We swap X and Y, and invert the new X (y, -x) to get a perfect 90-degree angle.
            // Because the forward direction is length 1, this is also length 1.
            double normalizedLateralDirX = normalizedForwardDirY;
            double normalizedLateralDirY = -normalizedForwardDirX;

            // STEP 5: APPLY THE PROPORTIONS (WEIGHTS)
            // We multiply the directions by your requested ratios (e.g., 10 forward, 5 sideways)
            double weightedForwardX = normalizedForwardDirX * flow;
            double weightedForwardY = normalizedForwardDirY * flow;

            double weightedLateralX = normalizedLateralDirX * lateral;
            double weightedLateralY = normalizedLateralDirY * lateral;

            // STEP 6: COMBINE THE WEIGHTED VECTORS
            // We add them together to get the raw diagonal vector.
            double combinedRawX = weightedForwardX + weightedLateralX;
            double combinedRawY = weightedForwardY + weightedLateralY;

            // STEP 7: NORMALIZE THE FINAL COMBINED VECTOR (Your brilliant correction!)
            // We find the length of this new combined vector...
            double combinedLength = Math.Sqrt((combinedRawX * combinedRawX) + (combinedRawY * combinedRawY));

            if (combinedLength > 0)
            {
                // ...and divide by its own length. 
                // Now, Vex and Vey represent a pure direction with a length of EXACTLY 1.
                Vex = combinedRawX / combinedLength;
                Vey = combinedRawY / combinedLength;
            }

            // Save the proportions so the TorrentLayer can read/modify them later
            FlowVector = flow;
            LateralVector = lateral;
        }
        else
        {
            /*Vex = 0;
            Vey = 0;*/
        }
    }
    
    public bool Equals(Vertex? other)
    {//.contains checks if the a vertex exists with the same coordinates
        return Cox == other.Cox && Coy == other.Coy;
    }
}