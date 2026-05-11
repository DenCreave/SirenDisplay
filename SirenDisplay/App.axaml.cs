using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Controllers;
using SirenDisplay.Views;

namespace SirenDisplay;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        
        services.AddSingleton<AnimatrixController>();
        services.AddSingleton<DotMapLoader>();
        services.AddSingleton<TorrentLayerLoader>();
        services.AddSingleton<DotMapLoader>();
        services.AddSingleton<ThemeBinderLoader>();
        services.AddSingleton<SpanningTreeController>();
        services.AddSingleton<SpanningTreeRenderer>();
        
        Services = services.BuildServiceProvider();

        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new InitView();
        }
        base.OnFrameworkInitializationCompleted();
    }
}