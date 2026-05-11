using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.Theme;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.DotMap;

public sealed class DotMapLoader
{
    public HashSet<IDotMapFactory> Maps { get; }
    public Dictionary<ThemeGroup, IDotMapFactory[]> DotMaps { get; }
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
                        && x.IsAssignableTo(typeof(IDotMapFactory)));

        try
        {
            foreach (var type in types)
            {
                /* using di now
                if (Activator.CreateInstance(type) is IDotMapFactory map)
                {
                    Maps.Add(map);
                }*/
                if (ActivatorUtilities.CreateInstance(App.Services, type) is IDotMapFactory dotMap)
                {
                    Maps.Add(dotMap);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        DotMaps = Maps.GroupBy(x=>x.Group)
            .ToDictionary(x=>x.Key, x=>x.OrderBy(y=>y.LayerLevel).ToArray());
    }
}