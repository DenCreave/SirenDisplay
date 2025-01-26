using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

using SirenDisplay.Assets.Polygons.Frames;

namespace SirenDisplay.ViewModels;

public sealed partial class ClockViewModel : ObservableObject
{
    
    [ObservableProperty] private Path _mypathFigures;
    public ClockViewModel()
    {
        
        //var backgroundGrid = this.FindControl<Grid>("BackGroundGrid"); 
        MypathFigures = new Path
        {

            Stroke =  new SolidColorBrush(Color.FromArgb(255, 0xff, 0x90, 0x1b)),
            StrokeThickness = 10,
            Margin = new Thickness(5),
            Stretch = Stretch.Fill,
            Data = new PathGeometry
            {
                Figures = new PathFigures
                {
                   new TopFrame().PathFigure,
                   new MiddleFrame().PathFigure,
                   new BottomFrame().PathFigure
                }
            },
            Effect = new DropShadowEffect
            {
                Color = Color.FromArgb(255, 0xff, 0x90, 0x1b),
                Opacity = 1,
                BlurRadius = 500
            },
        };
        Console.WriteLine("ClockViewModel");
        
    }
}