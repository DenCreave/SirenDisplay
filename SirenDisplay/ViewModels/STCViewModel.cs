using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Assets.Polygons.Frames;
using SirenDisplay.Controllers;
using SirenDisplay.Model;
using SirenDisplay.SpanningTree.TorrentLayer.Eye;

namespace SirenDisplay.ViewModels;

public sealed partial class STCViewModel :ViewModelBase
{
    [ObservableProperty] private Path _mainFrame;
    [ObservableProperty] private Path _mainFrame2;
    [ObservableProperty] private PathFigure _bg;
    [ObservableProperty] private LineSegment _aline;
    [ObservableProperty] private PathFigure _apoint;
    [ObservableProperty] private ArcSegment _aarc;
    [ObservableProperty] private ArcSegment _barc;
    [ObservableProperty] private Vertex _testpoint;
    private int radi = 20;
    private EyeTopTL _testLayer; // We need the layer for the test
    private PathFigure _guideLineFigure;
    

    public STCViewModel()
    {
        Console.WriteLine("STCViewModel init");
        _testLayer = new EyeTopTL(new AnimatrixController());
        /*Testpoint = new Vertex()
        {
            Cox = _testLayer.UniqueProps.TorrentPath[0].X,
            Coy = _testLayer.UniqueProps.TorrentPath[0].Y,
            TargetPathIndex = 1,
            //HaltonIndex = 1 // Uses the 0.5 noise we just set
        }.InitVertex();*/

        LineInitializer();
        FrameInitializer();
    }

    private void LineInitializer()
    {
        Aarc=new ArcSegment()
        {
            Point = new Point(30,20),
            
            Size=new Size(10,10),
            SweepDirection=SweepDirection.Clockwise,
            IsLargeArc=false,
        };
        Barc=new ArcSegment()
        {
            Point = new Point(10,20),
            
            Size=new Size(10,10),
            SweepDirection=SweepDirection.Clockwise,
            IsLargeArc=false,
        };
        
        Aline = new LineSegment
            { Point = new Point(50, 30) };
        Apoint = new PathFigure
        {
            StartPoint = new Point(10, 20),
            Segments =
            {
                Aarc,
                Barc
            }
        };
    }
    private void FrameInitializer()
    {
        _guideLineFigure = new PathFigure 
        {
            StartPoint = new Point(_testLayer.UniqueProps.TorrentPath[0].X, _testLayer.UniqueProps.TorrentPath[0].Y),
            IsClosed = false,
            IsFilled = false
        };

        // Add a line segment for every point in the TorrentPath
        for (int i = 0; i < _testLayer.UniqueProps.TorrentPath.Length; i++)
        {
            _guideLineFigure.Segments.Add(new LineSegment 
            { 
                Point = new Point(_testLayer.UniqueProps.TorrentPath[i].X, _testLayer.UniqueProps.TorrentPath[i].Y) 
            });
        }

        
        MainFrame = new Path
        {
            Stroke = Application.Current.FindResource("OffColor") as LinearGradientBrush,
            StrokeThickness = 10,
            Stretch = Stretch.Fill,
            Margin = new Thickness(5),
            Data = new PathGeometry
            {
                Figures = new PathFigures
                {
                    new TopFrame().PathFigure,
                    new MiddleFrame().PathFigure,
                    new BottomFrame().PathFigure,
                    new PathFigure(){StartPoint = new Point(40, 30), Segments = {Aline} }
                }
            },
            Effect = Application.Current.FindResource("OffEffect") as DropShadowEffect
        };
        MainFrame2 = new Path
        {
            StrokeThickness = 5,
            Stroke = Brushes.Cyan,
            Stretch = Stretch.Uniform,
            //Margin = new Thickness(5),
            Fill = Application.Current.FindResource("SirenColor") as LinearGradientBrush,
            Data = new PathGeometry
            {
                Figures = new PathFigures
                {
                    /// by giving it 2 points at the 2 opposing corners, it will set the width and
                    /// enables me to calculate anything in between
                    /// also a single point is not shown no matter the stroke thickness
                    new  PathFigure()
                    {
                        StartPoint = new Point(0, 0),
                    },
                    new PathFigure()
                    {
                        StartPoint = new Point(840, 480),
                    },
                    Apoint,
                    //Testpoint.Crest,
                    _guideLineFigure
                    
                }
            },
            //Effect = Application.Current.FindResource("OffEffect") as DropShadowEffect
        };
    }

    public async void PostInit()
    {
        Console.WriteLine("clicked");
        //Console.WriteLine($"der halton:{_testLayer.UniqueProps.Noise.HaltonValues1D[0]}");
        for (int i = 0; i < 500; i++)
        {
            //Aline.Point=new Point(Aline.Point.X+0.1,Aline.Point.Y-0.1);
            _testLayer.AffectVector(Testpoint);
            
            //Testpoint.UpdateCO().UpdateUI();
            Console.WriteLine($"lateral vector value: {Testpoint.LateralVector}");
            await Task.Delay(20);
        }

        Console.WriteLine(Aline);
         
    }
}