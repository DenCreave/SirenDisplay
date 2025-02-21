using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Path = Avalonia.Controls.Shapes.Path;

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

    private string _rootDir => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

    private string _currentDir { get; set; }
    
    [ObservableProperty] private DirectoryItem? _selected2Listen;
    [ObservableProperty] private DirectoryItem? _selectedDir;
    [ObservableProperty] private DirectoryItem? _selectedMusic;
    [ObservableProperty] private ObservableCollection<DirectoryItem> _directoryItems = new ObservableCollection<DirectoryItem>();
    [ObservableProperty] private ObservableCollection<DirectoryItem> _musicItems = new ObservableCollection<DirectoryItem>();

    public string PlayButton => IsPlayButton ? LabelData.PlayLabel : LabelData.StopLabel;

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(PlayButton))]
    private bool _isPlayButton;
    public MusicViewModel()
    {
        FrameInitializer();
        LabelData = new LabelData();
        //we might not have Cache references by that time
    }

    public void PostInit()
    {
        LoadPlayListNames(); 
        LoadPlaylistTitleNameGrid();
        LoadPlayListOptionsGrid();
        LoadPlaylistRenameGrid();
        InitDirPaths();
        InitMusicPaths();
        IsPlayButton = true;
        
        DirectoryItems.Clear();
        DirectoryItem tjmp = new DirectoryItem()
        {
            IsMusic = false,
            Label = LabelData.MusicLabel,
            Name = "tidk",
            FullPath = "/home/vava/Music/SOmething"
        };
        DirectoryItems.Add(tjmp);
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
    #region dircaller

    public void InitDirPaths()
    {
        _currentDir=_rootDir;
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
        DirectoryItems.AddRange(coll.OrderBy(x=>x.Name).ToArray());
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

    private async Task<List<DirectoryItem>> MusicLoader()
    {
        string[] music = Directory.GetFiles(_currentDir);
        List<DirectoryItem> coll = new();
        var libVLC = new LibVLC(enableDebugLogs: true);
        Media media = null;
        foreach (var item in music)
        {
            Console.WriteLine($"item is: {item}"); //i can add more tracks to it? 
            media=new Media(libVLC,item);
            Console.WriteLine($"the type: {media.Type}");
            await media.Parse();
            Console.WriteLine($"the type: {media.Type}");
            if (media.Tracks.Length==0)
            {
               // continue;
            }
            Console.WriteLine($"media.track was: {media.Tracks[0]}"); //i can add more tracks to it? 
            coll.Add(new DirectoryItem()
            {
                IsMusic = (media.Tracks[0].TrackType == TrackType.Audio || (media.Tracks[0].TrackType==TrackType.Video)?true : false),
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
        if (DirectoryItems.Count>0)
        {
            if (SelectedDir == null )
            {
                SelectedDir = DirectoryItems.Last();
                Selected2Listen = DirectoryItems.Last();
            }
            else
            {
                int index = DirectoryItems.IndexOf(SelectedDir);
                --index;
                if (index < 0)
                {
                    index = DirectoryItems.Count - 1;
                }
                SelectedDir=DirectoryItems[index];
                Selected2Listen=DirectoryItems[index];
            }
        }
    }

    public void DirectoryDown()
    {
        if (DirectoryItems.Count>0)
        {
            if (SelectedDir == null )
            {
                SelectedDir = DirectoryItems.First();
                Selected2Listen = DirectoryItems.First();
            }
            else
            {
                int index = DirectoryItems.IndexOf(SelectedDir);
                ++index;
                index %= DirectoryItems.Count;
                SelectedDir=DirectoryItems[index];
                Selected2Listen=DirectoryItems[index];
            }
        }
    }

    public void MusicPathUp()
    {
        if (MusicItems.Count>0)
        {
            if (SelectedMusic == null )
            {
                SelectedMusic = MusicItems.First();
                Selected2Listen = MusicItems.First();
            }
            else
            {
                int index = MusicItems.IndexOf(SelectedMusic);
                ++index;
                index %= MusicItems.Count;
                SelectedMusic=MusicItems[index];
                Selected2Listen=MusicItems[index];
            }
        }
    }

    

    public void MusicPathDown()
    {
        if (MusicItems.Count>0)
        {
            if (SelectedMusic == null )
            {
                SelectedMusic = MusicItems.First();
                Selected2Listen = MusicItems.First();
            }
            else
            {
                int index = MusicItems.IndexOf(SelectedMusic);
                --index;
                if (index < 0)
                {
                    index = MusicItems.Count - 1;
                }
                SelectedMusic=MusicItems[index];
                Selected2Listen=MusicItems[index];
            }
        }
    }

    public void SaveAndExit()
    {
        ConfController.SaveConf(CacheReferences.alarmTimerController.SirenData);
        SwitchToClockView(CacheReferences);
    }

    private void Save2SirenData()
    {//haha! in 1 line
        CacheReferences.alarmTimerController.SirenData.MusicPaths[_playlistNames[_playlistNameIndex]] = MusicItems.Select(x=>x.FullPath).ToList();
    }
    public void AddToPlaylist()
    {
        if (SelectedDir.IsMusic)
        {
            MusicItems.Add(SelectedDir);
            SelectedMusic = MusicItems.Last();
            Save2SirenData();
        }
    }

    public void RemoveFromPlaylist()
    {
        if (MusicItems.Count > 0)
        {
            MusicItems.Remove(SelectedMusic);
            Save2SirenData();
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