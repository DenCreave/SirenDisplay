using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SirenDisplay.Assets.SpanningTree.TorrentLayer;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Assets.SpanningTree.Theme;

public class ThemeBinderLoader
{
    public HashSet<IThemeBinder> Themes { get; }
    public Dictionary<ThemeGroup, IThemeBinder> ThemeDict { get; }
    public ThemeBinderLoader()
    {
        Themes = new();
        
        Assembly asm= Assembly.GetAssembly(typeof(ThemeBinderLoader));
        if (asm==null)
        {
            throw new NullReferenceException("ThemeBinderLoader assembly was NULL");
        }

        var types = asm.GetTypes()
            .Where(x => x.IsClass
                        && !x.IsAbstract
                        && x.IsAssignableTo(typeof(IThemeBinder)));

        try
        {
            foreach (var type in types)
            {
                if (ActivatorUtilities.CreateInstance(App.Services, type) is IThemeBinder theme)
                {
                    Themes.Add(theme);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        ThemeDict = Themes.ToDictionary(x=>x.ThemeGroup, x=>x);
    }
}