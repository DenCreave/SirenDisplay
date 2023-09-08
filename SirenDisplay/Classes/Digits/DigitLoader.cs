using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SirenDisplay.Interfaces;

namespace SirenDisplay.Classes.Digits;

public sealed class DigitLoader
{
    public Dictionary<int, IMyDigit> Digits { get; }

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
    }
}