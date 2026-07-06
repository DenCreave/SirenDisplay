using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SirenDisplay.SpanningTree.Theme;

namespace SirenDisplay.SpanningTree.UI;

public sealed class UIThemeLoader
{
    public HashSet<ISTTheme> UIThemes { get; }
    public Dictionary<ThemeGroup, ISTTheme> UIThemeDict { get; }

    public UIThemeLoader()
    {
        UIThemes = new();
        Assembly asm= Assembly.GetAssembly(typeof(UIThemeLoader));
        if (asm==null)
        {
            throw new NullReferenceException("UIThemeLoader assembly was NULL");
        }
        
        var types = asm.GetTypes()
            .Where(x => x.IsClass
                        && !x.IsAbstract
                        && x.IsAssignableTo(typeof(ISTTheme)));
        try
        {
            foreach (var type in types)
            {
                if (ActivatorUtilities.CreateInstance(App.Services, type) is ISTTheme theme)
                {
                    UIThemes.Add(theme);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        UIThemeDict = UIThemes.ToDictionary(x=>x.ThemeGroup, x=>x);
    }
}