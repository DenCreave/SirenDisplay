using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using SirenDisplay.Assets.SpanningTree.Controller;

namespace SirenDisplay.Assets.SpanningTree.UI;

public class SpanningTreeRenderer : Control
{
    /// <summary>
    /// this following is how the Frontend (as in AXAML) inject dependency.
    /// it inserts it in as a property, not as constructor.
    /// </summary>
    // 1. We register a  property that XAML is allowed to talk to
    // the reason its public is because Avalonia's engine needs to read it.
    // could call it XAML binding or XAML parsing
    public static readonly StyledProperty<SpanningTreeController> STCProperty =
        AvaloniaProperty.Register<SpanningTreeRenderer, SpanningTreeController>(nameof(STC));

    // 2. This is the actual property the XAML binding writes to!
    public SpanningTreeController STC
    {
        get => GetValue(STCProperty);
        set => SetValue(STCProperty, value);
    }
    
    public static readonly StyledProperty<SpanningTreeTheme> STTProperty =
        AvaloniaProperty.Register<SpanningTreeRenderer, SpanningTreeTheme>(nameof(STT));

    public SpanningTreeTheme STT
    {
        get => GetValue(STTProperty);
        set => SetValue(STTProperty, value);
    }

    
    private DispatcherTimer _renderTimer;

    public SpanningTreeRenderer()
    {
        _renderTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
        };

        _renderTimer.Tick += (sender, args) =>
        {
            if (STC == null) 
            { 
                Console.WriteLine("TICK FAILED: STC is null. Check XAML Binding."); 
                return; 
            }
            if (STT == null) 
            { 
                Console.WriteLine("TICK FAILED: STT is null. Check XAML Binding."); 
                return; 
            }
            
            double currentX = STC.CurrentScene[0].TorrentLayer.ResolutionNote.X;
            double currentY = STC.CurrentScene[0].TorrentLayer.ResolutionNote.Y;

            // If the engine changed resolution, update the canvas size!
            if (this.Width != currentX) this.Width = currentX;
            if (this.Height != currentY) this.Height = currentY;


            
            STC.UpdateFrame();
            // tells Avalonia: "The math changed redraw the screen!"
            // This automatically triggers the Render() method below.
            this.InvalidateVisual(); 
        };
    }

    // start the timer when the control appears on screen
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _renderTimer.Start();
    }

    // stop the timer if the control is removed (or if we close the window)
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _renderTimer.Stop();
    }

    
    public override void Render(DrawingContext context)
    {
        if (STC == null) { Debug.WriteLine("RENDER ABORTED: STC is NULL (Binding Failed)"); return; }
        if (STT == null) { Debug.WriteLine("RENDER ABORTED: STT is NULL (Binding Failed)"); return; }
        if (STC.CurrentScene == null) { Debug.WriteLine("RENDER ABORTED: CurrentScene is NULL"); return; }

        // If it reaches here, the engine is successfully running!
        Debug.WriteLine($"RENDER SUCCESS: Drawing {STC.CurrentScene.Length} layers at 60 FPS.");
        
        
        STT.DrawTheme(context, STC.CurrentScene, STC.CurrentTheme);
    }

}