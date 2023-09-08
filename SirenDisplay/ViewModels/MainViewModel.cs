using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Classes;
using SirenDisplay.Classes.Digits;

namespace SirenDisplay.ViewModels;

public  partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string _testObject = "ungabunga";
    
    
    
    [ObservableProperty] private string tesztNeked = " nukulalaalalkkakla";

    [ObservableProperty] private PathFigures _testGeometry = new PathFigures();

    public MainViewModel()
    {
       // helppo();
      // TestGeometry = new PathGeometry();
        UpdateTime();
    }


    private void UpdateTime()
    {
        var framerate = TimeSpan.FromSeconds(1 ); //i might just give it 1 frame per second so the cpu isnt burning away; original 1/60
        Console.WriteLine($"Das liege lüge frájmeráte: {framerate}");
        var timer = new DispatcherTimer
        {
            Interval = framerate
        };
       //  timer.Interval = framerate;
        int i = 0;
        

        /*
        var hahah = new MyTopPathFigure();
        var hahah2 = new MyTopLeftPathfigure();
        PathFigure temp1 = hahah.Figure;
        PathFigure temp2 = hahah2.Figure;

        PathFigures valami = new PathFigures{ { temp1 } };
        PathFigures valami2 = new PathFigures { { temp2 } };
        PathFigures valami3 = new PathFigures{ temp1,temp2};*/
        DigitLoader digitLoader = new DigitLoader();
        
        timer.Tick += (sender, args) =>
        {
            TestGeometry = digitLoader.Digits[i % 10].PathFigures;

            i++;
            
        };
        timer.Start();

       // var naezmi = valami.Figures;
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