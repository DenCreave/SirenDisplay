using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using SirenDisplay.Model;

namespace SirenDisplay.Controllers;

public sealed partial class AudioController : ObservableObject
{
    private LibVLC _libvlc {get; set;}
    private Media _media { get; set; } //= new Media(_libvlc, new Uri(@"C:\tmp\big_buck_bunny.mp4"));
    private MediaPlayer _mediaplayer { get; set; }//= new MediaPlayer(_media);
    //private List<DirectoryItem> _playlist { get; set; }
    private List<Media> _playlist { get; set; }
    
    private int  _playlistIndex { get; set; }
    private LabelData LabelData { get; }
    public string PlayButton => IsPlayButton ? LabelData.PlayLabel : LabelData.StopLabel;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(PlayButton))] private bool _isPlayButton;
    public AudioController()
    {
        _libvlc=new LibVLC(enableDebugLogs: true);
        _media = new Media(_libvlc, "tmp");
        _mediaplayer = new MediaPlayer(_media);
        LabelData=new LabelData();
        IsPlayButton = true;
    }

    public async Task PlayAudio(string path)
    {//i have to dispose of it tho, so...
        if (_mediaplayer.IsPlaying)
        {
            Stop();
        }
        _media=new Media(_libvlc,path);
        _mediaplayer = new MediaPlayer(_libvlc);
        _mediaplayer.Play(_media);
        IsPlayButton = false;
        _mediaplayer.EndReached += ToPlayIconTrue;
    }

    public async Task PlaySirenDisplay(List<DirectoryItem> paths)
    {
        if (_mediaplayer.IsPlaying)
        {
            Stop();
        }
        _mediaplayer.Stop();
        _playlist = new List<Media>();
        foreach (var path in paths)
        {
            _playlist.Add(new Media(_libvlc,path.FullPath));
        }

        _playlistIndex = 0;
        //_playlist = new List<DirectoryItem>(paths);
        _mediaplayer = new MediaPlayer(_libvlc); 
        _mediaplayer.EndReached += PlayNextSiren;
        //_media= new Media(_libvlc,_playlist.First().FullPath);
        Console.WriteLine($"Playing Siren Display first song {_playlist.First()}");
       // _playlist.RemoveAt(0);
        //_mediaplayer.Play(_media);
        _mediaplayer.Play(_playlist[_playlistIndex]);
        Console.WriteLine($"Playing Siren Display second song {_playlist[_playlistIndex].Tracks}");
        ++_playlistIndex;
    }

    public void ToPlayIconTrue(object sender, EventArgs e)
    {
        IsPlayButton = true;
    }

    public async Task PlayNextSiren()
    {
        if (_playlistIndex<_playlist.Count)
        {
            _mediaplayer = new MediaPlayer(_libvlc);
            _mediaplayer.EndReached += PlayNextSiren;
            _mediaplayer.Play(_playlist[_playlistIndex]);
            ++_playlistIndex;
        }
    }
    public async void PlayNextSiren(object sender, EventArgs e)
    {
        await PlayNextSiren();
        /*
        if (_playlist.Count != 0)
        {
            _media.Dispose();
            Console.WriteLine($"Playing Siren Display another song {_playlist.First().FullPath}");
            _media = new Media(_libvlc, _playlist.First().FullPath);
            _mediaplayer.Media=_media;
            _mediaplayer.Play();
            _playlist.RemoveAt(0);
        }
        else
        {
            Stop();
            Console.WriteLine("why are we stopping in the else ?");
        }*/
    }

    public bool IsPlaying()
    {
        if (_mediaplayer == null)
        {
            Console.WriteLine("No media player available it was null");
            return false;
        }
        return _mediaplayer.IsPlaying;
    }
    
    public void Stop()
    {
        //_mediaplayer.Dispose();
        _mediaplayer.Stop();
        IsPlayButton = true;
        Console.WriteLine("we stopped the music player in audiocontroller");
    }
}