using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using LibVLCSharp.Shared;
using SirenDisplay.Assets.Polygons.Frames;
using SirenDisplay.Controllers;
using SirenDisplay.Model;
using Color = Avalonia.Media.Color;
using Path = Avalonia.Controls.Shapes.Path;
using Rectangle = Avalonia.Controls.Shapes.Rectangle;

namespace SirenDisplay.ViewModels;

public sealed partial class MusicViewModel : ViewModelBase
{
    [ObservableProperty] private Path _mainFrame;
    public LabelData LabelData { get; }
    public CacheReferences CacheReferences { get; set; }

    [ObservableProperty] private Grid _playlistViewGrid = new();
    public Grid _playlistTitleNameGrid { get; set; }
    private Grid _playlisTitleOptionsGrid { get; set; }
    private Grid _playlisTitleRenameGrid { get; set; }
    [ObservableProperty] private Label _currentPlaylistTitle = new();
    private string[] _playlistNames { get; set; }
    private int _playlistNameIndex { get; set; }
    private TextBox _renameBox { get; set; }
    private string _rootDir => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

    private string _currentDir { get; set; }

    [ObservableProperty] private DirectoryItem? _selected2Listen;
    [ObservableProperty] private DirectoryItem? _selectedDir;
    private int _dirIndex { get; set; }
    [ObservableProperty] private DirectoryItem? _selectedMusic;
    private int _musicIndex { get; set; }

    [ObservableProperty]
    private ObservableCollection<DirectoryItem> _directoryItems = new ObservableCollection<DirectoryItem>();

    [ObservableProperty]
    private ObservableCollection<DirectoryItem> _musicItems = new ObservableCollection<DirectoryItem>();

    /// <summary>
    /// this function partial void On[PropertyName]Changed(T? value)
    /// is part of the mvvm toolkit, bruh this is so powerful, i love it
    /// </summary>
    /// <param name="value"></param>
    partial void OnSelectedDirChanged(DirectoryItem? value)
    {
        Selected2Listen = value;
    }

    partial void OnSelectedMusicChanged(DirectoryItem? value)
    {
        Selected2Listen = value;
    }

    public string PlayButton => IsPlayButton ? LabelData.PlayLabel : LabelData.StopLabel;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(PlayButton))]
    private bool _isPlayButton;

    public MusicViewModel()
    {
        FrameInitializer();
        LabelData = new LabelData();
        //we might not have Cache references by that time
    }

    public void PostInit()
    {
        InitMusicPaths();
        LoadPlaylistTitleNameGrid();
        LoadPlayListOptionsGrid();
        LoadPlaylistRenameGrid();
        SwapToNavigationMode(this,null);
        InitDirPaths();
        IsPlayButton = true;
        FetchPlaylist();
    }


    private void FrameInitializer()
    {
        MainFrame = new Path
        {
            Stroke = Application.Current.FindResource("OffColor") as LinearGradientBrush,
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
        _playlistTitleNameGrid = new Grid()
        {
            ColumnDefinitions = new ColumnDefinitions("*,*,*")
        };
        Color color = Color.Parse("#ff0090");
        SolidColorBrush brush = new SolidColorBrush(color);
        Viewbox viewbox1 = new Viewbox();
        Label arrowLeft = new Label
        {
            Foreground = brush,
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
            Foreground = brush,
            Classes = { "icon" },
            Content = LabelData.RightLabel
        };
        viewbox3.Child = arrowRight;
        viewbox3.PointerPressed += NextPlaylist;
        
        _playlistTitleNameGrid.Children.Add(viewbox1);
        _playlistTitleNameGrid.Children.Add(viewbox2);
        _playlistTitleNameGrid.Children.Add(viewbox3);
        
        Grid.SetColumn(viewbox1, 0);
        Grid.SetColumn(viewbox2, 1);
        Grid.SetColumn(viewbox3, 2);
        
    }

    private void LoadPlayListOptionsGrid()
    {
        _playlisTitleOptionsGrid = new Grid()
        {
            ColumnDefinitions = new ColumnDefinitions("*,*,*,*"),
        };
        Color colorB1 = Color.Parse("#ff901b");
        Color colorS1 = Color.Parse("#9e0022");
        SolidColorBrush brushB1 = new SolidColorBrush(colorB1);
        SolidColorBrush brushS1 = new SolidColorBrush(colorS1);
        Viewbox viewbox1 = new Viewbox();
        Label back = new Label()
        {
            Foreground = brushB1,
            Classes = { "icon" },
            Content = LabelData.BackLabel
        };
        viewbox1.Child = back;
        viewbox1.PointerPressed += SwapToNavigationMode;
        Viewbox viewbox2 = new Viewbox();
        Label addnew = new Label()
        {
            Foreground = brushB1,
            Classes = { "icon" },
            Content = LabelData.AddLabel
        };
        viewbox2.Child = addnew;
        viewbox2.PointerPressed += AddNewPlaylist;
        Viewbox viewbox3 = new Viewbox();
        Label edit = new Label()
        {
            Foreground = brushB1,
            Classes = { "icon" },
            Content = LabelData.EditLabel
        };
        viewbox3.Child = edit;
        viewbox3.PointerPressed += SwapToRenameMode;
        Viewbox viewbox4 = new Viewbox();
        Label delete = new Label()
        {
            Foreground = brushS1,
            Classes = { "icon" },
            Content = LabelData.DeleteLabel
        };
        viewbox4.Child = delete;
        viewbox4.PointerPressed += DeletePlaylist;
        
        _playlisTitleOptionsGrid.Children.Add(viewbox1);
        _playlisTitleOptionsGrid.Children.Add(viewbox2);
        _playlisTitleOptionsGrid.Children.Add(viewbox3);
        _playlisTitleOptionsGrid.Children.Add(viewbox4);
        
        Grid.SetColumn(viewbox1, 0);
        Grid.SetColumn(viewbox2, 1);
        Grid.SetColumn(viewbox3, 2);
        Grid.SetColumn(viewbox4, 3);
    }

    public void LoadPlaylistRenameGrid()
    {
        _playlisTitleRenameGrid = new Grid()
        {
            ColumnDefinitions = new ColumnDefinitions("*,*,*")
        };
        Viewbox viewbox1 = new Viewbox();
        Color colorB1 = Color.Parse("#ff901b");
        SolidColorBrush brushB1 = new SolidColorBrush(colorB1);
        Color colorG1 = Color.Parse("#39FF14");
        SolidColorBrush brushG1 = new SolidColorBrush(colorG1);
        Label back = new Label()
        {
            Foreground = brushB1,
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
            Foreground = brushG1,
            Classes = { "icon" },
            Content = LabelData.CheckLabel
        };
        viewbox3.Child = accept;
        viewbox3.PointerPressed += ConfirmRename;
        
        _playlisTitleRenameGrid.Children.Add(viewbox1);
        _playlisTitleRenameGrid.Children.Add(viewbox2);
        _playlisTitleRenameGrid.Children.Add(viewbox3);
        
        Grid.SetColumn(viewbox1, 0);
        Grid.SetColumn(viewbox2, 1);
        Grid.SetColumn(viewbox3, 2);
    }

    public void FetchPlaylist()
    {
        MusicItems.Clear();
        MusicItems.Add(
            CacheReferences.alarmTimerController.SirenData.MusicPaths[
                CacheReferences.alarmTimerController.SirenData.SelectedPlaylist]);
    }

    public void LoadPlayListNames()
    {
        //actualy i could just make this am interactive button with a usercontrol and just load it with the view locator..........
        //naaaah... its not that much of a mistake... right? is it a mistake tho? do i break pattern? either way its a good experience
        if (CacheReferences.alarmTimerController.SirenData.MusicPaths.Keys.Count == 0)
        {
            CacheReferences.alarmTimerController.SirenData.MusicPaths.Add("Siren Display", new List<DirectoryItem>());
            CacheReferences.alarmTimerController.SirenData.SelectedPlaylist = "Siren Display";
            Console.WriteLine("no playlist found, loading as Siren Display");
        }
            
        _playlistNames = CacheReferences.alarmTimerController.SirenData.MusicPaths.Keys.ToArray();
        _playlistNameIndex =
            _playlistNames.IndexOf(CacheReferences.alarmTimerController.SirenData.SelectedPlaylist);
        if(CacheReferences.alarmTimerController.SirenData.SelectedPlaylist =="")
        {
            CacheReferences.alarmTimerController.SirenData.SelectedPlaylist = _playlistNames[_playlistNameIndex];
        }
        Console.WriteLine("no playlist found, loading as Siren Display");
        

        Color color = Color.Parse("#ff901b");
        SolidColorBrush brush = new SolidColorBrush(color);
        CurrentPlaylistTitle = new Label()
        {
            Content = CacheReferences.alarmTimerController.SirenData.SelectedPlaylist,
            Foreground = brush,
        };
    }

    public void PreviousPlaylist(object sender, PointerPressedEventArgs e)
    {
        if (_playlistNameIndex == 0)
        {
            _playlistNameIndex = _playlistNames.Length - 1;
        }
        else
        {
            --_playlistNameIndex;
        }

        CurrentPlaylistTitle.Content = _playlistNames[_playlistNameIndex];
        CacheReferences.alarmTimerController.SirenData.SelectedPlaylist= _playlistNames[_playlistNameIndex];
        FetchPlaylist();
    }

    public void NextPlaylist(object sender, PointerPressedEventArgs e)
    {
        ++_playlistNameIndex;
        _playlistNameIndex %= _playlistNames.Length;
        CurrentPlaylistTitle.Content = _playlistNames[_playlistNameIndex];
        CacheReferences.alarmTimerController.SirenData.SelectedPlaylist= _playlistNames[_playlistNameIndex];
        //LoadMusicPaths();
        FetchPlaylist();
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
        CacheReferences.alarmTimerController.SirenData.MusicPaths.Add(tmp, new List<DirectoryItem>());
        CacheReferences.alarmTimerController.SirenData.SelectedPlaylist = tmp;
        _playlistNames = CacheReferences.alarmTimerController.SirenData.MusicPaths.Keys.ToArray();
        _playlistNameIndex = _playlistNames.IndexOf(CacheReferences.alarmTimerController.SirenData.SelectedPlaylist);
        CurrentPlaylistTitle.Content = _playlistNames[_playlistNameIndex];
        FetchPlaylist();
        SwapToNavigationMode(this, null);
        
    }

    public void DeletePlaylist(object sender, PointerPressedEventArgs e)
    {
        CacheReferences.alarmTimerController.SirenData.MusicPaths.Remove(_playlistNames[_playlistNameIndex]);
        _playlistNames = CacheReferences.alarmTimerController.SirenData.MusicPaths.Keys.ToArray();
        if (_playlistNames.Length == 0)
        {
            _playlistNames = ["Siren Display"];
            CacheReferences.alarmTimerController.SirenData.MusicPaths.Add(_playlistNames[0], new List<DirectoryItem>());
            CacheReferences.alarmTimerController.SirenData.SelectedPlaylist = _playlistNames[0];
        }

        _playlistNameIndex = 0;
        CacheReferences.alarmTimerController.SirenData.SelectedPlaylist = _playlistNames[_playlistNameIndex];
        _playlistNameIndex = _playlistNames.IndexOf(CacheReferences.alarmTimerController.SirenData.SelectedPlaylist);
        CurrentPlaylistTitle.Content = _playlistNames[_playlistNameIndex];
        FetchPlaylist();
        SwapToNavigationMode(this,null);
    }

    public void ConfirmRename(object sender, PointerPressedEventArgs e)
    {
        //todo maybe i should do a regex also
        if (_renameBox.Text is null)
        {
            throw new NullReferenceException("_renameBox was null after confirmation");
        }

        CacheReferences.alarmTimerController.SirenData.MusicPaths.Add(_renameBox.Text,
            CacheReferences.alarmTimerController.SirenData.MusicPaths[_playlistNames[_playlistNameIndex]]);
        CacheReferences.alarmTimerController.SirenData.MusicPaths.Remove(_playlistNames[_playlistNameIndex]);
        CacheReferences.alarmTimerController.SirenData.SelectedPlaylist = _renameBox.Text;
        _playlistNames[_playlistNameIndex] = _renameBox.Text;
        CurrentPlaylistTitle.Content = _playlistNames[_playlistNameIndex];
        FetchPlaylist();
        SwapToNavigationMode(sender, e);
    }

    #endregion DynamicBar

    //i just realized its almost 300 lines here, this is going to be a godclass ffs, i definitely break pattern

    #region dircaller

    public void InitDirPaths()
    {
        _currentDir = _rootDir;
        LoadDirectoryItems();
    }

    public void InitMusicPaths()
    {
        LoadMusicPaths();
    }

    private async void LoadDirectoryItems()
    {
        List<DirectoryItem> coll = new();
        DirectoryItems.Clear();
        coll.Add(DirectoryLoader());
        var tmp = await Task.WhenAll(MusicLoader());
        foreach (var tem in tmp)
        {
            coll.Add(tem);
        }

        DirectoryItems.AddRange(coll.OrderBy(x => x.Name).ToArray());
        SelectedMusic = null;
    }

    private List<DirectoryItem> DirectoryLoader()
    {
        string[] dirs = Directory.GetDirectories(_currentDir);
        List<DirectoryItem> coll = new();
        foreach (var item in dirs)
        {
            coll.Add(new DirectoryItem()
            {
                IsMusic = false,
                Label = LabelData.FolderLabel,
                Name = System.IO.Path.GetFileName(item),
                FullPath = item
            });
        }
        
        return coll;
    }
    //todo what doesnt work atm is the swapping the playlistcontent with the playlistnames, so thats next
    private async Task<List<DirectoryItem>> MusicLoader()
    {
        string[] music = Directory.GetFiles(_currentDir);
        List<DirectoryItem> coll = new();
        var libVLC = new LibVLC(enableDebugLogs: false); //todo enable
        Media media = null;
        foreach (var item in music)
        {
            Console.WriteLine($"item is: {item}"); //i can add more tracks to it? 
            media = new Media(libVLC, item);
            Console.WriteLine($"the type: {media.Type}");
            await media.Parse();
            Console.WriteLine($"the type: {media.Type}");
            if (media.Tracks.Length == 0)
            {
                continue;
            }

            Console.WriteLine($"media.track was: {media.Tracks[0]}"); //i can add more tracks to it? 
            coll.Add(new DirectoryItem()
            {
                IsMusic = (media.Tracks[0].TrackType == TrackType.Audio ||
                           (media.Tracks[0].TrackType == TrackType.Video)
                    ? true
                    : false),
                Label = LabelData.MusicLabel,
                Name = System.IO.Path.GetFileName(item),
                FullPath = item
            });
        }

        SelectedMusic = null;
        return coll;
    }

    #endregion dircaller

    #region musiccaller

    public void LoadMusicPaths()
    {
        CacheReferences.alarmTimerController.SirenData = ConfController.LoadConf();
    }

    #endregion musiccaller

    #region MusicHandlingButtons

    public void DirectoryUp()
    {
        if (DirectoryItems.Count > 0)
        {
            if (SelectedDir == null)
            {
                SelectedDir = DirectoryItems.Last();
            }
            else
            {
                int index = DirectoryItems.IndexOf(SelectedDir);
                --index;
                if (index < 0)
                {
                    index = DirectoryItems.Count - 1;
                }

                SelectedDir = DirectoryItems[index];
            }
        }
    }

    public void DirectoryDown()
    {
        if (DirectoryItems.Count > 0)
        {
            if (SelectedDir == null)
            {
                SelectedDir = DirectoryItems.First();
            }
            else
            {
                int index = DirectoryItems.IndexOf(SelectedDir);
                ++index;
                index %= DirectoryItems.Count;
                SelectedDir = DirectoryItems[index];
                Selected2Listen = DirectoryItems[index];
            }
        }
    }

    public void MusicPathUp()
    {
        if (MusicItems.Count > 0)
        {
            if (SelectedMusic == null)
            {
                SelectedMusic = MusicItems.First();
            }
            else
            {
                int index = MusicItems.IndexOf(SelectedMusic);
                --index;
                if (index < 0)
                {
                    index = MusicItems.Count - 1;
                }

                SelectedMusic = MusicItems[index];
            }
        }
    }


    public void MusicPathDown()
    {
        if (MusicItems.Count > 0)
        {
            if (SelectedMusic == null)
            {
                SelectedMusic = MusicItems.First();
            }
            else
            {
                int index = MusicItems.IndexOf(SelectedMusic);
                ++index;
                index %= MusicItems.Count;
                SelectedMusic = MusicItems[index];
            }
        }
    }

    public void SaveAndExit()
    {
        ConfController.SaveConf(CacheReferences.alarmTimerController.SirenData);
        SwitchToClockView(CacheReferences);
    }

    private void Save2SirenData()
    {
        //haha! in 1 line
        CacheReferences.alarmTimerController.SirenData.MusicPaths[_playlistNames[_playlistNameIndex]] = MusicItems.ToList();
    }

    public void AddToPlaylist()
    {
        if (SelectedDir.IsMusic)
        {
            MusicItems.Add(new DirectoryItem(SelectedDir));
            SelectedMusic = MusicItems.Last();
            Save2SirenData();
        }
        else
        {
            _currentDir = Selected2Listen.FullPath;
            LoadDirectoryItems();
        }
    }

    public void RemoveFromPlaylist()
    {
        if (Selected2Listen != null && Selected2Listen == SelectedMusic)
        {
            if (MusicItems.Count > 0)
            {
                MusicItems.Remove(SelectedMusic);
                Save2SirenData();
            }
        }
    }

    public void UpADir()
    {
        if (_currentDir != _rootDir)
        {
            Console.WriteLine($"_currentDir.LastIndexOf('/'){_currentDir.LastIndexOf('/')}");
            _currentDir = _currentDir.Substring(0, _currentDir.LastIndexOf('/'));
            Console.WriteLine($"_currentDir: {_currentDir}");
            LoadDirectoryItems();
        }
    }

    public async void PlayStopMedia()
    {
        if (CacheReferences.alarmTimerController.AudioController.IsPlaying())
        {
            CacheReferences.alarmTimerController.AudioController.Stop();
            IsPlayButton = true;
        }
        else
        {
            await CacheReferences.alarmTimerController.AudioController.PlayAudio(Selected2Listen.FullPath);
            //is this even supposed to work like this? will it be responsive?
            IsPlayButton = false;
        }
    }

    #endregion MusicHandlingButtons
}