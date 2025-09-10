using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.Controller;

public sealed class STCLoader
{
    public HashSet<ISTController>  Controller { get; } 
    public Dictionary<STCGroup, ISTController[]> STCControllers { get; }
    public STCLoader()
    {
        Controller = new ();
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
                    Controller.Add(controller);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        STCControllers = Controller.GroupBy(x=>x.Group)
            .ToDictionary(x=>x.Key, x=>x.OrderBy(y=>y.Name).ToArray());
    }
}