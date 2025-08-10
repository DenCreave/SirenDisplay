using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.DotMap;

public sealed class DotMapLoader
{
    public HashSet<IDotMap> Maps { get; }
    public IDotMap[] DotMaps { get; }
    
    /* public PathGeometry ReturnPathGeometry( int index )
     {
         return PathGeometries[index];
     }*/
    
    public DotMapLoader()
    {
        Maps = new();
        
        Assembly asm= Assembly.GetAssembly(typeof(DotMapLoader));
        if (asm==null)
        {
            throw new NullReferenceException("DotMapLoader assembly was NULL");
        }

        var types = asm.GetTypes()
            .Where(x => x.IsClass
                        && !x.IsAbstract
                        && x.IsAssignableTo(typeof(IDotMap)));

        try
        {
            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is IDotMap map)
                {
                    Maps.Add(map);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        DotMaps = Maps.OrderBy(x=>x.ID).ToArray();
    }
}