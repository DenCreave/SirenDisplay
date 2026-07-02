using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Threading;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.Controller;

public sealed class SpanningTreeController
{
    private readonly ThemeBinderLoader _themeBinderLoader;
    private Stopwatch _frameTimer;
    private bool _restartRequested;
    private bool _loadNextTheme;

    public ThemeGroup CurrentTheme { get; set; }
    public Animap[] CurrentScene { get; set; }
    public SpanningTreeController(ThemeBinderLoader themeBinderLoader)
    {
        _themeBinderLoader = themeBinderLoader;
        _restartRequested = false;
        _loadNextTheme = false;
        _frameTimer = Stopwatch.StartNew(); 
        CurrentTheme = ThemeGroup.Eye;
        CurrentScene = themeBinderLoader.ThemeDict[CurrentTheme].ElicitateAnimap();
    }

    public void LoadNextTheme()
    {
        ++CurrentTheme;
        CurrentScene = _themeBinderLoader.ThemeDict[CurrentTheme].ElicitateAnimap();
        _frameTimer.Restart();
    }

    public void RestartLayers()
    {
        foreach (var animap in CurrentScene)
        {
            animap.TorrentLayer.Reset();
        }
        _restartRequested = false;

    }

    public void RequestRestart()
    {
        _restartRequested = true;
    }

    public void RequestNextTheme()
    {
        _loadNextTheme = true;
    }

    public void UpdateFrame()
    {
        if (_loadNextTheme)
        {
            LoadNextTheme();
        }

        if (_restartRequested)
        {
            RestartLayers();
        }
        
        double dt = _frameTimer.Elapsed.TotalSeconds;
        _frameTimer.Restart();
        foreach (var animap in CurrentScene)
        {
            animap.TorrentLayer.UpdateState(dt);
            
            Parallel.ForEach(animap.Graph.Vertices, vertex =>
            {
                animap.TorrentLayer.AffectVector(vertex);
                // 2. APPLY the physics to the coordinates!
                if (vertex.IsEnabled) 
                {
                    vertex.UpdateCO(); 
                }
            });
        }
    }
}