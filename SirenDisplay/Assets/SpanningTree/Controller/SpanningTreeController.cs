using System.Collections.Generic;
using System.Threading.Tasks;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.Controller;

public sealed class SpanningTreeController
{
    private readonly ThemeBinderLoader _themeBinderLoader;
    public ThemeGroup CurrentTheme { get; set; }
    public Animap[] CurrentScene { get; set; }
    public SpanningTreeController(ThemeBinderLoader themeBinderLoader)
    {
        _themeBinderLoader = themeBinderLoader;
        CurrentTheme = ThemeGroup.Eye;
        CurrentScene = themeBinderLoader.ThemeDict[CurrentTheme].ElicitateAnimap();
        foreach(var animap in CurrentScene)
        {
            animap.TorrentLayer.Init();
        }
    }

    public void LoadNextTheme()
    {
        foreach(var animap in CurrentScene)
        {
            animap.TorrentLayer.Reset(); 
        }
        
        ++CurrentTheme;
        CurrentScene = _themeBinderLoader.ThemeDict[CurrentTheme].ElicitateAnimap();
        
        foreach(var animap in CurrentScene)
        {
            animap.TorrentLayer.Init();
        }
    }

    public void UpdateFrame()
    {
        foreach (var animap in CurrentScene)
        {
            Parallel.ForEach(animap.Graph.Vertices, vertex => 
            {
                if (vertex.IsEnabled)
                {
                    animap.TorrentLayer.AffectVector(vertex);
                }
            });
        }
    }
}