using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia.Media;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes.Digits;

public sealed class DigitLoader
{
    public Dictionary<int, IMyDigit> Digits { get; }
    public PathGeometry[] PathGeometries { get; }
    
    public PathGeometry ReturnPathGeometry( int index )
    {
        return PathGeometries[index];
    }
    
    public DigitLoader()
    {
        Digits = new();
        
        Assembly asm= Assembly.GetAssembly(typeof(DigitLoader));
        if (asm==null)
        {
            throw new NullReferenceException("Skill loader assembly was NULL");
        }

        var types = asm.GetTypes()
            .Where(x => x.IsClass
                        && !x.IsAbstract
                        && x.IsAssignableTo(typeof(IMyDigit)));

        try
        {
            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is IMyDigit digit)
                {
                    Digits.Add(digit.ID, digit);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
        PathGeometries = new PathGeometry[Digits.Count+1]; //PathGeometries[10] is an empty pathgeometry
        for (int i = 0; i < 10; i++)
        {
            PathGeometries[i] = new PathGeometry
            {
                Figures = Digits[i].PathFigures
            };
        }

        PathGeometries[10] = new PathGeometry()
        {
            Figures = new PathFigures()
        };
    }
}