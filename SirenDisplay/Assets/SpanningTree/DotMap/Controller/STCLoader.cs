using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.DotMap.Controller;

public sealed class STCLoader
{
    public HashSet<ISTController>  Controllers { get; } 
    public Dictionary<STCGroup, ISTController[]> STCController { get; }
    public STCLoader()
    {
        Controllers = new ();
        Assembly asm= Assembly.GetAssembly(typeof(STCLoader));
        if (asm==null)
        {
            throw new NullReferenceException("STCLoader assembly was NULL");
        }
        var types = asm.GetTypes()
            .Where(x => x.IsClass 
                        && !x.IsAbstract 
                        && x.IsAssignableTo(typeof(ISTController)));
        try
        {
            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is ISTController controller)
                {
                    Controllers.Add(controller);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        STCController = Controllers.GroupBy(x=>x.Group)
            .ToDictionary(x=>x.Key, x=>x.OrderBy(y=>y.DotMap.LayerLevel).ToArray());
    }
}