using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SirenDisplay.ViewModels;

public  partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string _testObject = "ungabunga";
    
    
    
    [ObservableProperty] private string tesztNeked = " nukulalaalalkkakla";

    public MainViewModel()
    {
       // helppo();
    }

    private async Task helppo()
    {
        await Task.Run(async () =>
        {
            await Task.Delay(1500);
            TestObject= "alkjslkslskkkkkkkkkkkkkkkkkk";
            TesztNeked = "aÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁÁ";
            for (int i = 0; i < 5; i++)
            {
                TesztNeked += "b2"; //this works
                //tesztNeked += "x1";
                await Task.Delay(500);
                Console.WriteLine($"help {i}");
                    
            }
        });
    }
}