using System.Linq;
using Avalonia;
using Avalonia.Media;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Model;

namespace SirenDisplay.Assets.SpanningTree.UI.Eye;

public class EyeStyle
{
    public ResNote ResolutionNote { get; }
    public LinearGradientBrush BGLayer { get; }
    // public RadialGradientBrush BGGlow { get; }
    public RadialGradientBrush[] GlowPalette { get; }

    public double DotRadius { get; } 
    public double DotGlowRadius { get; }
    public double DotGlowRatio { get; }
    public int LUTCount { get; }
    
    //layer 1
    public IPen EmeraldGlow { get; }
    public IPen EmeraldCore { get; }
    public DashStyle EmeraldDash { get; }

    public IPen AmethystArcGlow { get; }
    public IPen AmethystArcCore { get; }
    public DashStyle AmethystDashArc { get; }
    public DashStyle AmethystDashGlowArc { get; }

    public IPen AmethystEdgeGlow { get; }
    public IPen AmethystEdgeCore { get; }
    public DashStyle AmethystDashEdge { get; }
    
    //layer 2 
    public IPen IrisEdgeGlow { get; }
    public IPen IrisEdgeCore { get; }
    public DashStyle IrisEdgeDash { get; }

    public IPen IrisArcGlow { get; }
    public IPen IrisArcCore { get; }
    public DashStyle IrisArcDash { get; }
    
    public IPen IrisConnectGlow { get; }
    public IPen IrisConnectCore { get; }


    public EyeStyle(TorrentLayerLoader loader)
    {
        ResolutionNote = loader.CreateNewLayer(ThemeGroup.Eye, TLName.Top).ResolutionNote;

        Color colorStart = Color.FromArgb(255, 255, 0, 144);  // Pink
        Color colorEnd = Color.FromArgb(255, 255, 109, 61);   // Orange

        // todo add an enviroment variable for resolution.
        BGLayer = new LinearGradientBrush()
        {
            // using relativeunit.Absolute here cos the coloring applies to all
            // dots as one.
            StartPoint = new RelativePoint(0, ResolutionNote.Y, RelativeUnit.Absolute),
            EndPoint = new RelativePoint(ResolutionNote.X, 0, RelativeUnit.Absolute),
            Opacity = 0.8, //note: will be multiplied by fader
            GradientStops = new GradientStops
            {
                new GradientStop(colorStart, 0.0),
                //new GradientStop(Color.Parse("#ff0090"), 0.0),
                new GradientStop(colorEnd, 1.0),
                //new GradientStop(Color.Parse("#ff6d3d"), 1.0)
            }
        };
        LUTCount = 100;
        GlowPalette = new RadialGradientBrush[LUTCount];
        DotRadius = 2.5;
        DotGlowRatio = 3;
        DotGlowRadius = DotRadius * DotGlowRatio;
        GeneratePalette(colorStart, colorEnd);

        /*BGGlow = new RadialGradientBrush()
        {
            // Center the gradient perfectly in the middle of the circle
            // using relativeunit.relative here cos the coloring applies to each
            // dot themselves.
            Center = new RelativePoint(0.5, 0.5, RelativeUnit.Relative),
            GradientOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative),
            Radius = 0.5, // a gradient radius of 0.5 means the gradient
                          // starts in the center and ends exactly at the
                          // outer edge of the ellipse. It won't get clipped,
                          // and it won't fall short.

            GradientStops = new GradientStops
            {
                new GradientStop(Color.FromArgb(200, 255, 109, 61), DotRadius/DotGlowRadius),
                new GradientStop(Color.FromArgb(0, 255, 109, 61), 1.0),
            }
        };*/
        
        
        // LAYER 1 INITIALIZATION
        // A thick, semi-transparent pen for the neon glow / shadow effect
        // draw 10 lines, then skip 2, then draw 2, and skip 2. { 10, 2, 2, 2 }
        EmeraldDash = new DashStyle(new double[] { 10, 2, 2, 2 },0);
        
        EmeraldGlow = new Pen(
            brush: new SolidColorBrush(Color.FromArgb(150, 15, 84, 15)),
            thickness: 6.0,
            lineCap: PenLineCap.Flat, 
            lineJoin: PenLineJoin.Round,
            dashStyle: null);
        
        // NOTE: line joins 
        // Miter: Sharp, pointy corners. (Great for the thin Core).
        // Round: Soft, sanded-down corners. (Great for the thick Halo, so the glow doesn't create weird, spiky artifacts at the joints).
        // Bevel: Chiseled, flat corners.
        EmeraldCore = new Pen(
            brush: new SolidColorBrush(Color.FromArgb(255, 57, 255, 20)), 
            thickness: 1.5,
            lineCap: PenLineCap.Flat, 
            lineJoin: PenLineJoin.Miter,
            dashStyle: EmeraldDash);
            
        
        double[] basePattern = new double[] 
            { 5,2,2,3, 1, 3,2,2,4,2,2,3,1,3,2,2};
        AmethystDashArc = new DashStyle(basePattern, 0);
        double[] glowPattern = basePattern.Select(x=> x/3).ToArray();
        AmethystDashGlowArc = new DashStyle(glowPattern, 0);
        
        AmethystDashEdge = new DashStyle(new double[]
            { 7,1,4,1}, 0);

        //amethyst arc
        AmethystArcGlow = new Pen(
            brush: new SolidColorBrush(Color.FromArgb(120, 255, 0, 144)), 
            thickness: 4.5,
            lineCap: PenLineCap.Round,
            lineJoin: PenLineJoin.Round,
            dashStyle: AmethystDashGlowArc);

       
        AmethystArcCore = new Pen(
            brush: new SolidColorBrush(Color.FromArgb(240, 255, 0, 144)),
            //brush: new SolidColorBrush(Color.FromArgb(255, 255, 0, 150)), // Bright pale orange/white
            thickness: 1.5,
            lineCap: PenLineCap.Round,
            lineJoin: PenLineJoin.Round,
            dashStyle: AmethystDashArc);
        
        //amethyst edge
        AmethystEdgeGlow = new Pen(
            brush: new SolidColorBrush(Color.FromArgb(100, 255, 0, 144)), 
            thickness: 5,
            lineCap: PenLineCap.Flat,
            lineJoin: PenLineJoin.Round,
            dashStyle: null);

        
        AmethystEdgeCore = new Pen(
            brush: new SolidColorBrush(Color.FromArgb(255, 149, 0, 89)),
            //brush: new SolidColorBrush(Color.FromArgb(255, 255, 0, 150)), // Bright pale orange/white
            thickness: 1.5,
            lineCap: PenLineCap.Flat, 
            lineJoin: PenLineJoin.Miter,
            dashStyle: AmethystDashEdge);
        
        //iris
        IrisEdgeDash = new DashStyle(new double[]
            { 11,1,7,1},0);
        IrisArcDash = new DashStyle(new double[]
            { 1,2,1,2,3,3,1,3,3,2,1,2,}, 0);
        
        IrisEdgeGlow =new Pen(
            brush: new SolidColorBrush(Color.FromArgb(100, 255, 12, 89)), 
        thickness: 6.0,
        lineCap: PenLineCap.Round,
        lineJoin: PenLineJoin.Round,
        dashStyle: DashStyle.Dash);
        
        IrisEdgeCore =new Pen(
            brush: new SolidColorBrush(Color.FromArgb(255, 255, 109, 61)), 
            thickness: 1.5,
            lineCap: PenLineCap.Flat,
            lineJoin: PenLineJoin.Miter,
            dashStyle: IrisEdgeDash);
        
        IrisArcGlow = new Pen(
            brush: new SolidColorBrush(Color.FromArgb(100, 255, 12, 89)), 
            thickness: 6.0,
            lineCap: PenLineCap.Round,
            lineJoin: PenLineJoin.Round,
            dashStyle: IrisArcDash);
        
        IrisArcCore =new Pen(
            brush: new SolidColorBrush(Color.FromArgb(255, 255, 109, 61)), 
            thickness: 1.5,
            lineCap: PenLineCap.Flat,
            lineJoin: PenLineJoin.Miter,
            dashStyle: IrisArcDash);

        IrisConnectGlow = new Pen(
            brush: new SolidColorBrush(Color.FromArgb(90, 255, 12, 89)),
            thickness: 6.0,
            lineCap: PenLineCap.Round,
            lineJoin: PenLineJoin.Round,
            dashStyle: DashStyle.Dash);
        
        IrisConnectCore =new Pen(
            brush: new SolidColorBrush(Color.FromArgb(230, 255, 109, 61)), 
            thickness: 1.5,
            lineCap: PenLineCap.Flat,
            lineJoin: PenLineJoin.Miter,
            dashStyle: DashStyle.Dash);
        
    }
    

    private void GeneratePalette(Color colorStart, Color colorEnd)
    {
        for (int i = 0; i < LUTCount; i++)
        {
            double amount = i / (double)LUTCount; // 0.0 to 1.0
            Color color = InterpolateColor(colorStart, colorEnd, amount);
            
            // Create the transparent versions for the hollow glow
            

            GlowPalette[i] = new RadialGradientBrush()
            {
                Center = new RelativePoint(0.5, 0.5, RelativeUnit.Relative),
                GradientOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative),
                Radius = 0.5,
                GradientStops = new GradientStops
                {
                    new GradientStop(Color.FromArgb(0, color.R, color.G, color.B),0.0),
                    // 200 glow at 1:3 (glow ratio was 3 when this comment was written)
                    new GradientStop(Color.FromArgb(200, color.R, color.G, color.B), DotRadius/DotGlowRadius),
                    // 1 third of glow at 2:3 (glow ratio was 3 when this comment was written)
                    new GradientStop(Color.FromArgb((200/3), color.R, color.G, color.B), (((DotGlowRadius-DotRadius)/3)+DotRadius)/DotGlowRadius),
                    new GradientStop(Color.FromArgb(0, color.R, color.G, color.B), 1.0),
                }
            };
        }
    }
    
    private Color InterpolateColor(Color c1, Color c2, double amount)
    {
        byte r = (byte)(c1.R + (c2.R - c1.R) * amount);
        byte g = (byte)(c1.G + (c2.G - c1.G) * amount);
        byte b = (byte)(c1.B + (c2.B - c1.B) * amount);
        return Color.FromArgb(255, r, g, b);
    }
}