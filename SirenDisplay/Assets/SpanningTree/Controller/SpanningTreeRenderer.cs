using Avalonia.Controls;
using Avalonia.Media;

namespace SirenDisplay.Assets.SpanningTree.Controller;

public class SpanningTreeRenderer : Control
{
    public override void Render(DrawingContext context)
    {
        if (STC == null || STC.CurrentScene == null) return;

        // todo, over write the UI renderer here, massive class, tick updates etc
    }
}