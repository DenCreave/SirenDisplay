using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SirenDisplay.Interfaces;
using SirenDisplay.SpanningTree.Theme;

namespace SirenDisplay.SpanningTree.TorrentLayer;

public sealed class TorrentLayerLoader
{
    // private HashSet<ITorrentLayer> Layers { get; }
    // private Dictionary<ThemeGroup, ITorrentLayer[]> TorrentLayers { get; }
    private Dictionary<(ThemeGroup, TLName), Type> LayerBlueprints { get; } = new();

    public TorrentLayerLoader()
    {
        var types = Assembly.GetAssembly(typeof(TorrentLayerLoader))
            .GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(typeof(ITorrentLayer)));

        foreach (var type in types)
        {
            if (ActivatorUtilities.CreateInstance(App.Services, type) is ITorrentLayer dummy)
            {
                // Store the Type using the dummys name, dummy is an instance.
                LayerBlueprints.Add((dummy.Group, dummy.Name), type);
            }
        }
    }
    
    public ITorrentLayer CreateNewLayer(ThemeGroup theme, TLName name)
    {
        Type layerType = LayerBlueprints[(theme,name)];
        
        // i stored the type, i need to instantiate it. its an object, so convert
        return (ITorrentLayer)ActivatorUtilities.CreateInstance(App.Services, layerType);
    }
}