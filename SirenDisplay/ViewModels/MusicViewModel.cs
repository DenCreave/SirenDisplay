using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using SirenDisplay.Assets.Polygons.Frames;
using SirenDisplay.Model;

namespace SirenDisplay.ViewModels;

public sealed partial class MusicViewModel : ViewModelBase
{
    [ObservableProperty] private Path _mainFrame;
    public LabelData LabelData { get;}
    public CacheReferences CacheReferences { get; set; }

    [ObservableProperty] private Grid _playlistViewGrid;
    private Grid _playlistTitleNameGrid {get; set;}
    private Grid _playlisTitleOptionsGrid { get; set;}
    [ObservableProperty] private Label _currentPlaylistTitle;
    private string[] _playlistNames { get; set; }
    private int _playlistNameIndex { get; set; }
    private bool _isToggled { get; set; }
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
        LoadPlayListNames();
        viewbox2.Child = CurrentPlaylistTitle;
        viewbox2.PointerPressed += SwapToEditMode;
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

    private void LoadPlayListEditGrid()
    {
        _playlisTitleOptionsGrid = new Grid()
        {
            ColumnDefinitions = new ColumnDefinitions("*,*,*,*"),
        };
        Viewbox viewbox1 = new Viewbox();
        Label back = new Label()
        {
            Foreground = Application.Current.FindResource("B1") as LinearGradientBrush,
            Classes = { "icon" },
            Content = LabelData.BackLabel
        };
        viewbox1.Child = back;
        viewbox1.PointerPressed += SwapToNavigationMode;
        Viewbox viewbox2 = new Viewbox();
        Label addnew = new Label()
        {
            Foreground = Application.Current.FindResource("B1") as LinearGradientBrush,
            Classes = { "icon" },
            Content = LabelData.AddLabel
        };
        viewbox2.Child = addnew;
        //todo add new playslist, then a rename aaand a delete
        viewbox2.PointerPressed += AddNewPlaylist;
        Viewbox viewbox3 = new Viewbox();
        Label edit = new Label()
        {
            Foreground = Application.Current.FindResource("B1") as LinearGradientBrush,
            Classes = { "icon" },
            Content = LabelData.EditLabel
        };
        viewbox3.Child = edit;
        viewbox3.PointerPressed += RenamePlaylist;
        Viewbox viewbox4 = new Viewbox();
        Label delete = new Label()
        {
            Foreground = Application.Current.FindResource("S1") as LinearGradientBrush,
            Classes = { "icon" },
            Content = LabelData.DeleteLabel
        };
        viewbox4.Child = delete;
        viewbox4.PointerPressed += DeletePlaylist;
    }

    public void LoadPlayListNames()
    {
        //actualy i could just make this am interactive button with a usercontrol and just load it with the view locator..........
        //naaaah... its not that much of a mistake... right? is it a mistake tho? do i break pattern? either way its a good experience
        if (CacheReferences.alarmTimerController.SirenData.MusicPaths.Keys.Count > 0)
        {
            _playlistNames = CacheReferences.alarmTimerController.SirenData.MusicPaths.Keys.ToArray();
            _playlistNameIndex = _playlistNames.IndexOf(CacheReferences.alarmTimerController.SirenData.SelectedPlaylist);
        }
        else
        {
            _playlistNames = ["Siren Display"];
            _playlistNameIndex = 0;
            Console.WriteLine("no playlist found, loading as Siren Display");
        }

        CurrentPlaylistTitle = new Label()
        {
            Content = _playlistNames[_playlistNameIndex],
            Foreground = Application.Current.FindResource("B1") as LinearGradientBrush
        };
        Console.WriteLine($"for debug reason\tname:{_playlistNames}\tindex:{_playlistNameIndex}");
        _isToggled = false;

    }
    public void PreviousPlaylist(object sender, PointerPressedEventArgs e)
    {
        if (_playlistNameIndex==0)
        {
            _playlistNameIndex = _playlistNames.Length-1;
        }
        else
        {
            --_playlistNameIndex;
        }
        CurrentPlaylistTitle.Content = _playlistNames[_playlistNameIndex];
    }

    public void NextPlaylist(object sender, PointerPressedEventArgs e)
    {
        ++_playlistNameIndex;
        _playlistNameIndex%=_playlistNames.Length;
        CurrentPlaylistTitle.Content = _playlistNames[_playlistNameIndex];
    }

    public void SwapToEditMode(object sender, PointerPressedEventArgs e)
    {
        throw new NotImplementedException("oopsie");
    }

    public void SwapToNavigationMode(object sender, PointerPressedEventArgs e)
    {
        throw new NotImplementedException("oopsie");
    }

    public void AddNewPlaylist(object sender, PointerPressedEventArgs e)
    {
        throw new NotImplementedException("oopsie");
    }

    public void RenamePlaylist(object sender, PointerPressedEventArgs e)
    {
        throw new NotImplementedException("oopsie");
    }

    public void DeletePlaylist(object sender, PointerPressedEventArgs e)
    {
        throw new NotImplementedException("oopsie");
    }
}