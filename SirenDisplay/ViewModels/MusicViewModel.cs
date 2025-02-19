using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
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
    private Grid _playlisTitleRenameGrid { get; set;}
    [ObservableProperty] private Label _currentPlaylistTitle;
    private string[] _playlistNames { get; set; }
    private int _playlistNameIndex { get; set; }
    private TextBox _renameBox { get; set; }
    public MusicViewModel()
    {
        FrameInitializer();
        LoadPlayListNames(); //we might not have Cache references by that time
        LoadPlaylistTitleNameGrid();
        LoadPlayListOptionsGrid();
        LoadPlaylistRenameGrid();
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

    #region DynamicBar
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
        viewbox2.PointerPressed += SwapToOptionsMode;
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

    private void LoadPlayListOptionsGrid()
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
        viewbox2.PointerPressed += AddNewPlaylist;
        Viewbox viewbox3 = new Viewbox();
        Label edit = new Label()
        {
            Foreground = Application.Current.FindResource("B1") as LinearGradientBrush,
            Classes = { "icon" },
            Content = LabelData.EditLabel
        };
        viewbox3.Child = edit;
        viewbox3.PointerPressed += SwapToRenameMode;
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

    public void LoadPlaylistRenameGrid()
    {
        _playlisTitleRenameGrid = new Grid()
        {
            ColumnDefinitions = new ColumnDefinitions("*,*,*")
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
        _renameBox = new TextBox()
        {
            Text = _playlistNames[_playlistNameIndex] //todo remember to refresh it when switching playlists 
        };
        viewbox2.Child = _renameBox;
        Viewbox viewbox3 = new Viewbox();
        Label accept = new Label()
        {
            Foreground = Application.Current.FindResource("G1") as LinearGradientBrush,
            Classes = { "icon" },
            Content = LabelData.CheckLabel
        };
        viewbox3.Child = accept;
        viewbox3.PointerPressed += ConfirmRename;
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
            CacheReferences.alarmTimerController.SirenData.SelectedPlaylist = _playlistNames[_playlistNameIndex];
            Console.WriteLine("no playlist found, loading as Siren Display");
        }
        
        CurrentPlaylistTitle = new Label()
        {
            Content = _playlistNames[_playlistNameIndex],
            Foreground = Application.Current.FindResource("B1") as LinearGradientBrush
        };
        Console.WriteLine($"for debug reason\tname:{_playlistNames}\tindex:{_playlistNameIndex}");

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
        LoadMusicPaths();
    }

    public void NextPlaylist(object sender, PointerPressedEventArgs e)
    {
        ++_playlistNameIndex;
        _playlistNameIndex%=_playlistNames.Length;
        CurrentPlaylistTitle.Content = _playlistNames[_playlistNameIndex];
        LoadMusicPaths();
    }

    public void SwapToOptionsMode(object sender, PointerPressedEventArgs e)
    {
        PlaylistViewGrid = _playlisTitleOptionsGrid;
    }

    public void SwapToNavigationMode(object sender, PointerPressedEventArgs e)
    {
        PlaylistViewGrid = _playlistTitleNameGrid;
    }

    public void SwapToRenameMode(object sender, PointerPressedEventArgs e)
    {
        PlaylistViewGrid = _playlisTitleRenameGrid;
    }

    public void AddNewPlaylist(object sender, PointerPressedEventArgs e)
    {
        string tmp = $"Siren Display {_playlistNames.Length}";
        CacheReferences.alarmTimerController.SirenData.MusicPaths.Add(tmp,new List<string>());
        CacheReferences.alarmTimerController.SirenData.SelectedPlaylist = tmp;
        _playlistNames = CacheReferences.alarmTimerController.SirenData.MusicPaths.Keys.ToArray();
        _playlistNameIndex = _playlistNames.IndexOf(CacheReferences.alarmTimerController.SirenData.SelectedPlaylist);
        CurrentPlaylistTitle.Content = _playlistNames[_playlistNameIndex];
    }
    

    public void DeletePlaylist(object sender, PointerPressedEventArgs e)
    {
        CacheReferences.alarmTimerController.SirenData.MusicPaths.Remove(_playlistNames[_playlistNameIndex]);
        _playlistNames = CacheReferences.alarmTimerController.SirenData.MusicPaths.Keys.ToArray();
        if (_playlistNames.Length == 0)
        {
            _playlistNames = ["Siren Display"];
            CacheReferences.alarmTimerController.SirenData.MusicPaths.Add(_playlistNames[0],new List<string>());
        }
        
        _playlistNameIndex = 0;
        CacheReferences.alarmTimerController.SirenData.SelectedPlaylist = _playlistNames[_playlistNameIndex];
        _playlistNameIndex = _playlistNames.IndexOf(CacheReferences.alarmTimerController.SirenData.SelectedPlaylist);
    }

    public void ConfirmRename(object sender, PointerPressedEventArgs e)
    {
        //todo maybe i should do a regex also
        if (_renameBox.Text is null)
        {
            throw new NullReferenceException("_renameBox was null after confirmation");
        }
        CacheReferences.alarmTimerController.SirenData.MusicPaths.Add(_renameBox.Text, CacheReferences.alarmTimerController.SirenData.MusicPaths[_playlistNames[_playlistNameIndex]]);
        CacheReferences.alarmTimerController.SirenData.MusicPaths.Remove(_playlistNames[_playlistNameIndex]);
        CacheReferences.alarmTimerController.SirenData.SelectedPlaylist = _renameBox.Text;
        _playlistNames[_playlistNameIndex] = _renameBox.Text;
        CurrentPlaylistTitle.Content = _playlistNames[_playlistNameIndex];
        SwapToNavigationMode(sender, e);
    }

    #endregion DynamicBar
    //i just realized its almost 300 lines here, this is going to be a godclass ffs, i definitely break pattern
    
    
    public void LoadMusicPaths()
    {//into the right side of the2nd gridrow
        throw new NotImplementedException("oopsie");
    }
    
    #region MusicHandlingButtons

    public void DirectoryUp()
    {
        throw new NotImplementedException("oopsie");    
    }

    public void DirectoryDown()
    {
        throw new NotImplementedException("oopsie");
    }

    public void MusicPathUp()
    {
        throw new NotImplementedException("oopsie");
    }

    public void MusicPathDown()
    {
        throw new NotImplementedException("oopsie");
    }

    public void SaveAndExit()
    {
        throw new NotImplementedException("oopsie");
    }

    public void AddToPlaylist()
    {
        throw new NotImplementedException("oopsie");
    }

    public void RemoveFromPlaylist()
    {
        throw new NotImplementedException("oopsie");
    }

    public void PlayStopMedia()
    {
        throw new NotImplementedException("oopsie");
    }
        
        
    #endregion MusicHandlingButtons
}