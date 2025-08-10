using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia.Media;
using SirenDisplay.Classes.Digits;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.TorrentLayer;

public sealed class TorrentLayerLoader
{
    public HashSet<ITorrentLayer> Layers { get; }
    public ITorrentLayer[] TorrentLayers { get; }
    
   /* public PathGeometry ReturnPathGeometry( int index )
    {
        return PathGeometries[index];
    }*/
    
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
                if (Activator.CreateInstance(type) is ITorrentLayer layer)
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
        
        TorrentLayers = Layers.OrderBy(x=>x.ID).ToArray();
    }
}