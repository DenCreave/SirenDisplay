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
        
        foreach (var animap in CurrentScene)
        {
            double dt = _frameTimer.Elapsed.TotalSeconds;
            _frameTimer.Restart();
            animap.TorrentLayer.UpdateState(dt);
            
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