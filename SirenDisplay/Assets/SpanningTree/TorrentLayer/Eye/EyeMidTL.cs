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
    public bool IsAffecting => UniqueProps.FaderConf.IsAlive;
    public bool IsVisible { get; }

    public EyeMidTL(AnimatrixController controls)
    {
        //IsAffecting = true;
        IsVisible = true;
        Controls = controls;
        Align = RenderAlignment.ScreenCenter;
        UniqueProps = new MidIrisProperties(0.3);
        UniqueProps.FaderConf = new Fader()
        {
            FadeInDelay = 1.5,
            FadeInDuration = 2,
            Lifetime = 30,
            FadeOutDelay = 0,
            FadeOutDuration = 2,
            LayerOpacity = 0,
            TotalElapsedSeconds = 0
        };
    }

    public void UpdateState(double deltaTime)
    {
        UniqueProps.FaderConf.UpdateTime(deltaTime);
    }

    public void Reset()
    {
        UniqueProps.FaderConf.Reset();
    }

    public void Spawn(Vertex vertex)
    {
        vertex.IsEnabled = true;
    }

    public void Despawn(Vertex vertex)
    {
        vertex.IsEnabled = false;
    }

    public void AffectVector(Vertex vertex)
    {
        if (UniqueProps.FaderConf.IsAlive)
        {
            if (!vertex.IsEnabled)
            {
                Spawn(vertex);
            }
            Controls.Rotate(vertex, UniqueProps.RadiansPerFrame);
        }
        else
        {
            if (vertex.IsEnabled)
            {
                Despawn(vertex);
            }
        }
    }
}