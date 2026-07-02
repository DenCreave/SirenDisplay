using System;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Media;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.TLProps;

namespace SirenDisplay.Assets.SpanningTree.UI.Eye;

public sealed class EyeVortexTheme(StyleArchive styleArchive) : ISTTheme
{
    public ThemeGroup ThemeGroup { get; } = ThemeGroup.Eye;
    public StyleArchive StyleArchive { get; } = styleArchive;

    public void Draw(DrawingContext context, Animap[] currentScene)
    {
        foreach (var animap in currentScene)
        {
            var layer = animap.TorrentLayer;
            if (!layer.IsVisible) continue;
            bool isCentered = layer.Align == RenderAlignment.ScreenCenter;


            double currentOpacity = 1.0;

            // If this layer uses MidIrisProperties, we safely unwrap it and read the Fader directly!
            if (layer is ITorrentLayer<MidIrisProperties> midLayer)
            {
                currentOpacity = midLayer.UniqueProps.FaderConf.LayerOpacity;
            }


            // We use a using block so the transform is automatically popped off 
            // the GPU when this specific layer is done drawing.
            using (context.PushOpacity(currentOpacity))
            using (isCentered
                       ? context.PushTransform(Matrix.CreateTranslation(
                           StyleArchive.EyeStyle.ResolutionNote.X / 2.0,
                           StyleArchive.EyeStyle.ResolutionNote.Y / 2.0))
                       : default)
            {
                if (animap.Graph.LayerLevel == 0)
                {
                    DrawLayer0(context, animap);
                }
                else if (animap.Graph.LayerLevel == 1)
                {
                    DrawLayer1(context, animap);
                }
                else if (animap.Graph.LayerLevel == 2)
                {
                    DrawLayer2(context, animap);
                }
            }
        }
    }

    private void DrawLayer2(DrawingContext context, Animap animap)
    {
        StreamGeometry sapphireArcGeo = null, rubyArcGeo = null, rubyEdgeGeo = null;
        StreamGeometryContext sapphireArcPen = null, rubyArcPen = null, rubyEdgePen = null;

        // 1. BUILD THE GEOMETRIES
        foreach (var edge in animap.Graph.Edges)
        {
            //if (!edge.IsEnabled) continue;

            StreamGeometryContext activePen = null;

            switch (edge.Group)
            {
                case Insignia.Sapphire:
                    if (sapphireArcGeo == null)
                    {
                        sapphireArcGeo = new StreamGeometry();
                        sapphireArcPen = sapphireArcGeo.Open();
                    }

                    activePen = sapphireArcPen;
                    break;

                case Insignia.Ruby:
                    if (edge.RelationType == EdgeRelType.Arc)
                    {
                        if (rubyArcGeo == null)
                        {
                            rubyArcGeo = new StreamGeometry();
                            rubyArcPen = rubyArcGeo.Open();
                        }

                        activePen = rubyArcPen;
                    }
                    else
                    {
                        if (rubyEdgeGeo == null)
                        {
                            rubyEdgeGeo = new StreamGeometry();
                            rubyEdgePen = rubyEdgeGeo.Open();
                        }

                        activePen = rubyEdgePen;
                    }

                    break;
            }

            var pointA = new Point(edge.A.Cox, edge.A.Coy);
            var pointB = new Point(edge.B.Cox, edge.B.Coy);

            activePen.BeginFigure(pointA, isFilled: false);

            if (edge.RelationType == EdgeRelType.Line)
            {
                activePen.LineTo(pointB);
            }
            else if (edge.RelationType == EdgeRelType.Arc)
            {
                if (edge.Group == Insignia.Sapphire)
                {
                    // The connecting chains form a perfect circle
                    DrawConcentricArc(activePen, pointA, pointB);
                }
                else if (edge.Group == Insignia.Ruby)
                {
                    // The star internals bow inward (using 0.66 ratio)
                    DrawInwardArc(activePen, pointA, pointB, 0.66);
                }
            }

            activePen.EndFigure(isClosed: false);
        }

        // 2. CLOSE CONTEXTS AND DRAW TO GPU
        // for cleare bloom!
        // A. Finalize all geometries first
        if (sapphireArcPen != null) sapphireArcPen.Dispose();
        if (rubyArcPen != null) rubyArcPen.Dispose();
        if (rubyEdgePen != null) rubyEdgePen.Dispose();

        // B. PASS 1: Draw ALL Glows (Background)
        if (sapphireArcPen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.IrisConnectGlow, sapphireArcGeo);
        if (rubyArcPen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.IrisArcGlow, rubyArcGeo);
        if (rubyEdgePen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.IrisEdgeGlow, rubyEdgeGeo);

        // C. PASS 2: Draw ALL Cores (Foreground)
        if (sapphireArcPen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.IrisConnectCore, sapphireArcGeo);
        if (rubyArcPen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.IrisArcCore, rubyArcGeo);
        if (rubyEdgePen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.IrisEdgeCore, rubyEdgeGeo);
    }

    /// <summary>
    /// Draws an arc that bows INWARD towards the center. Used for the Star internals.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DrawInwardArc(StreamGeometryContext ctx, Point pointA, Point pointB, double ratio)
    {
        double distanceX = pointB.X - pointA.X;
        double distanceY = pointB.Y - pointA.Y;
        double pixelDistance = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));

        // 0.66 creates a nice, tight inward curve
        double curveRadius = pixelDistance * ratio;

        // The logical center of the constellation is 0,0 because of PushTransform
        double midpointX = pointA.X + (distanceX / 2.0);
        double midpointY = pointA.Y + (distanceY / 2.0);

        // Cross Product to determine Left/Right of the origin (0,0)
        double leftOrRightTest = (distanceX * midpointY) - (distanceY * midpointX);

        // Bow INWARD
        var curveDirection = leftOrRightTest > 0 ? SweepDirection.Clockwise : SweepDirection.CounterClockwise;

        ctx.ArcTo(pointB, new Size(curveRadius, curveRadius), 0, false, curveDirection);
    }

    /// <summary>
    /// Draws an arc that perfectly traces a circle around the origin (0,0). Used for the connecting chains.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DrawConcentricArc(StreamGeometryContext ctx, Point pointA, Point pointB)
    {
        // To make a perfect circle, the radius of the arc MUST be the distance from the point to the origin (0,0)
        double radius = Math.Sqrt((pointA.X * pointA.X) + (pointA.Y * pointA.Y));

        double distanceX = pointB.X - pointA.X;
        double distanceY = pointB.Y - pointA.Y;

        double midpointX = pointA.X + (distanceX / 2.0);
        double midpointY = pointA.Y + (distanceY / 2.0);

        // Cross Product to determine Left/Right of the origin (0,0)
        double leftOrRightTest = (distanceX * midpointY) - (distanceY * midpointX);

        // INVERTED from the inward arc! We want it to bow OUTWARD to trace the perfect circle.
        var curveDirection = leftOrRightTest > 0 ? SweepDirection.CounterClockwise : SweepDirection.Clockwise;

        ctx.ArcTo(pointB, new Size(radius, radius), 0, false, curveDirection);
    }

    private void DrawLayer1(DrawingContext context, Animap animap)
    {
        // Explicitly declare our geometries and contexts for Layer 1
        StreamGeometry emeraldGeo = null, amethystArcGeo = null, amethystEdgeGeo = null;
        StreamGeometryContext emeraldPen = null, amethystArcPen = null, amethystEdgePen = null;

        // 1. BUILD THE GEOMETRIES
        foreach (var edge in animap.Graph.Edges)
        {
            // Skip dead edges
            //if (!edge.IsEnabled) continue;

            StreamGeometryContext activePen = null;

            // Route the edge to the correct geometry context based on its Insignia Group
            switch (edge.Group)
            {
                case Insignia.Emerald:
                    if (emeraldGeo == null)
                    {
                        emeraldGeo = new StreamGeometry();
                        emeraldPen = emeraldGeo.Open();
                    }

                    activePen = emeraldPen;
                    break;

                case Insignia.Amethyst:
                    if (edge.RelationType == EdgeRelType.Arc)
                    {
                        if (amethystArcGeo == null)
                        {
                            amethystArcGeo = new StreamGeometry();
                            amethystArcPen = amethystArcGeo.Open();
                        }

                        activePen = amethystArcPen;
                    }
                    else
                    {
                        if (amethystEdgeGeo == null)
                        {
                            amethystEdgeGeo = new StreamGeometry();
                            amethystEdgePen = amethystEdgeGeo.Open();
                        }

                        activePen = amethystEdgePen;
                    }

                    break;
            }

            // (Fail-Fast: If activePen is null here, it will intentionally crash on BeginFigure)

            var pointA = new Point(edge.A.Cox, edge.A.Coy);
            var pointB = new Point(edge.B.Cox, edge.B.Coy);

            // Start drawing the segment
            activePen.BeginFigure(pointA, isFilled: false);

            if (edge.RelationType == EdgeRelType.Line)
            {
                activePen.LineTo(pointB);
            }
            else if (edge.RelationType == EdgeRelType.Arc)
            {
                // Layer 1 uses the inward arc. 
                // 0.75 gives a nice, smooth, shallow curve for the outer Iris web.
                DrawInwardArc(activePen, pointA, pointB, 0.3);
            }

            activePen.EndFigure(isClosed: false);
        }

        // 2. CLOSE CONTEXTS AND DRAW TO GPU
        // for clearer bloom!
        // A. Finalize all geometries first
        if (emeraldPen != null) emeraldPen.Dispose();
        if (amethystArcPen != null) amethystArcPen.Dispose();
        if (amethystEdgePen != null) amethystEdgePen.Dispose();

        // B. PASS 1: Draw ALL Glows (Background)
        if (emeraldPen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.EmeraldGlow, emeraldGeo);
        if (amethystArcPen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.AmethystArcGlow, amethystArcGeo);
        if (amethystEdgePen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.AmethystEdgeGlow, amethystEdgeGeo);

        // C. PASS 2: Draw ALL Cores (Foreground)
        if (emeraldPen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.EmeraldCore, emeraldGeo);
        if (amethystArcPen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.AmethystArcCore, amethystArcGeo);
        if (amethystEdgePen != null) context.DrawGeometry(null, StyleArchive.EyeStyle.AmethystEdgeCore, amethystEdgeGeo);
    }


    private void DrawLayer0(DrawingContext context, Animap animap)
    {
        double x = StyleArchive.EyeStyle.ResolutionNote.X;
        double y = StyleArchive.EyeStyle.ResolutionNote.Y;
        double lengthSq = (x * x) + (y * y);
        // bg bloom first then dots
        foreach (var dot in animap.Graph.Vertices)
        {
            if (!dot.IsEnabled) continue;
            var glowBrush = GetGlowBrush(dot, x, y, lengthSq);
            context.DrawEllipse(glowBrush, null, new Point(dot.Cox, dot.Coy), 
                StyleArchive.EyeStyle.DotGlowRadius, StyleArchive.EyeStyle.DotGlowRadius);
        }

        // bg bloom first, then dots.
        foreach (var dot in animap.Graph.Vertices)
        {
            if (!dot.IsEnabled) continue;
            context.DrawEllipse(StyleArchive.EyeStyle.BGLayer, null, new Point(dot.Cox, dot.Coy), 
                StyleArchive.EyeStyle.DotRadius, StyleArchive.EyeStyle.DotRadius);
        }
    }

    /// <summary>
    /// Calculates the vector projection to find the correct glow color from the palette.
    /// AggressiveInlining ensures zero method-call overhead in the 60 FPS loop.
    ///
    /// This tells the C# JIT compiler to physically paste the math back
    /// into the loop during runtime, completely eliminating the method call overhead.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private RadialGradientBrush GetGlowBrush(Vertex dot, double x, double y, double lengthSq)
    {
        // Vector from StartPoint (0, y) to EndPoint (x, 0) is (x, -y)
        // Vector from StartPoint to Dot is (dot.Cox, dot.Coy - y)
        double dotProduct = (dot.Cox * x) + ((dot.Coy - y) * (-y));
        double t = dotProduct / lengthSq;

        // Clamp 't' between 0.0 and 1.0 just in case a dot flies off-screen
        t = Math.Clamp(t, 0.0, 1.0); // cleaner than nested Max/Min

        // Convert 't' to an index between 0 and 99
        int paletteIndex = (int)(t * (StyleArchive.EyeStyle.LUTCount - 1));

        return StyleArchive.EyeStyle.GlowPalette[paletteIndex];
    }
}