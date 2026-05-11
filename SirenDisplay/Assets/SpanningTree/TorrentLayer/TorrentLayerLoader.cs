using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer;

public sealed class TorrentLayerLoader
{
    public HashSet<ITorrentLayer> Layers { get; }
    public Dictionary<ThemeGroup, ITorrentLayer[]> TorrentLayers { get; }
    public TorrentLayerLoader()
    {
        Layers = new();
        
        Assembly asm= Assembly.GetAssembly(typeof(TorrentLayerLoader));
        if (asm==null)
        {
            throw new NullReferenceException("TorrentLayerLoader assembly was NULL");
        }

        var types = asm.GetTypes()
            .Where(x => x.IsClass
                        && !x.IsAbstract
                        && x.IsAssignableTo(typeof(ITorrentLayer)));

        try
        {
            foreach (var type in types)
            {
                /* im using DI now, and they have params
                if (Activator.CreateInstance(type) is ITorrentLayer layer)
                {
                    Layers.Add(layer);
                }*/
                if (ActivatorUtilities.CreateInstance(App.Services, type) is ITorrentLayer layer)
                {
                    Layers.Add(layer);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        TorrentLayers = Layers.GroupBy(x=>x.Group)
           .ToDictionary(x=>x.Key, x=>x.OrderBy(y=>y.Name).ToArray());
    }
}