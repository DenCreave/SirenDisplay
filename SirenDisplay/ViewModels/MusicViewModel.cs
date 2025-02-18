using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Assets.Polygons.Frames;
using SirenDisplay.Model;

namespace SirenDisplay.ViewModels;

public sealed partial class MusicViewModel : ViewModelBase
{
    [ObservableProperty] private Path _mainFrame;
    public LabelData LabelData { get; set; }
    public CacheReferences CacheReferences { get; set; }

    [ObservableProperty] private Grid _playlistViewGrid;
    private Grid _playlistTitleNameGrid {get; set;}
    private Grid _playlisTitleOptionsGrid { get; set;}
    [ObservableProperty] private Label _currentPlaylistTitle;
    private string[] _playlistNames { get; set; }
    public MusicViewModel()
    {
        FrameInitializer();
        LoadPlayListNames(); //we might not have Cache references by that time
    }
    private void FrameInitializer()
    {
        MainFrame = new Path
        {
            Stroke =  Application.Current.FindResource("OffColor") as LinearGradientBrush,
            StrokeThickness = 10,
            Stretch = Stretch.Fill,
            Data = new PathGeometry
            {
                Figures = new PathFigures
                {
                    new TopFrameSquare().PathFigure,
                    new MiddleFrame().PathFigure,
                    new BottomFrameSquare().PathFigure
                }
            },
            Effect = Application.Current.FindResource("OffEffect") as DropShadowEffect
        };
    }

    
    
    private void LoadPlaylistTitleNameGrid()
    {
        //todo check if it works at all
        _playlistTitleNameGrid = new Grid()
        {
            ColumnDefinitions = new ColumnDefinitions("*,*,*")
        };
        Viewbox viewbox1 = new Viewbox();
        Label arrowLeft = new Label
        {
            
            Foreground = Application.Current.FindResource("A2") as LinearGradientBrush,
            Classes = { "icon" },
            Content = LabelData.LeftLabel
        };
        viewbox1.Child = arrowLeft;
        viewbox1.PointerPressed += PreviousPlaylist;
        Viewbox viewbox2 = new Viewbox();
        
        Viewbox viewbox3 = new Viewbox();
        Label arrowRight = new Label()
        {
            Foreground = Application.Current.FindResource("A2") as LinearGradientBrush,
            Classes = { "icon" },
            Content = LabelData.RightLabel
        };
        viewbox3.Child = arrowRight;
        viewbox3.PointerPressed += NextPlaylist;

    }

    public void LoadPlayListNames()
    {
        //actualy i could just make this am interactive button with a usercontrol and just load it with the view locator..........
        //naaaah... its not that much of a mistake... right? is it a mistake tho? do i break pattern? either way its a good experience
        if (CacheReferences.alarmTimerController.SirenData.MusicPaths.Keys.Count > 0)
        {
            _playlistNames = CacheReferences.alarmTimerController.SirenData.MusicPaths.Keys.ToArray();
        }
        else
        {
            _playlistNames = ["Siren Display"];
            Console.WriteLine("no playlist found, loading as Siren Display");
        }

        Console.WriteLine($"for debug reason {_playlistNames}");
        
    }
    public void PreviousPlaylist(object sender, PointerPressedEventArgs e)
    {
        throw new NotImplementedException("work work");
    }

    public void NextPlaylist(object sender, PointerPressedEventArgs e)
    {
        throw new NotImplementedException("ungabunga");
    }
}