using System;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Controllers;
using SirenDisplay.Interfaces;
using SirenDisplay.Model;
using SirenDisplay.Model.TLProps;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer.Eye;

public sealed class EyeMidTL : ITorrentLayer<MidIrisProperties>
{
    public ThemeGroup Group { get; } = ThemeGroup.Eye;
    public TLName Name { get; } = TLName.Mid;
    public ResNote ResolutionNote { get; } = new ResNote() { X = 800, Y = 480, Ratio = "5:3" };
    public RenderAlignment Align { get; }
    public MidIrisProperties UniqueProps { get; }

    public AnimatrixController Controls { get; }

    public EyeMidTL(AnimatrixController controls)
    {
        Controls = controls;
        Align = RenderAlignment.ScreenCenter;
        UniqueProps = new MidIrisProperties(0.3);
    }

    public void Init()
    {
        throw new System.NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Spawn(Vertex vertex)
    {
        // todo, await animation, like 3 secs await and then fade in
    }

    public void Despawn(Vertex vertex)
    {
        //todo after x time, fade out tho prolly handled by thetheme binder stc controller
    }

    public void AffectVector(Vertex vertex)
    {
        Controls.Rotate(vertex, UniqueProps.RadiansPerFrame);
    }
}