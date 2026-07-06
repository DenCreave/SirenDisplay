using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using SirenDisplay.Model;

namespace SirenDisplay.Controllers;

public sealed partial class AudioController : ObservableObject
{
    private LibVLC _libvlc { get; set; }
    private Media _media { get; set; } 
    private MediaPlayer _mediaplayer { get; set; }
    private List<Media> _playlist { get; set; }
    
    private int _playlistIndex { get; set; }
    private LabelData LabelData { get; }
    public string PlayButton => IsPlayButton ? LabelData.PlayLabel : LabelData.StopLabel;

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(PlayButton))] 
    private bool _isPlayButton;

    public AudioController()
    {
        _libvlc = new LibVLC(enableDebugLogs: false);
        LabelData = new LabelData();
        IsPlayButton = true;
    }
    
    // c++ objects require direct cleanup. libvlc seems to be a c++ code 
    // with c# wrapper around it. gc cant see it.
    private void CleanupCurrentMedia()
    {
        if (_mediaplayer != null)
        {
            _mediaplayer.Stop();
            _mediaplayer.EndReached -= ToPlayIconTrue;
            _mediaplayer.EndReached -= PlayNextSiren;
            _mediaplayer.Dispose(); // Kills the C++ Player
            _mediaplayer = null;
        }

        if (_media != null)
        {
            _media.Dispose(); // Kills the C++ Media
            _media = null;
        }
    }

    public async Task PlayAudio(string path)
    {
        CleanupCurrentMedia();

        // 2. Create the new C++ objects
        _media = new Media(_libvlc, path);
        _mediaplayer = new MediaPlayer(_libvlc);
        //Race condition 101 - always subscribe to events before calling them
        _mediaplayer.EndReached += ToPlayIconTrue;
        _mediaplayer.Play(_media);
        IsPlayButton = false;
    }

    public async Task PlaySirenDisplay(List<DirectoryItem> paths)
    {
        CleanupCurrentMedia();
        if (_playlist != null)
        {
            foreach (var media in _playlist) media.Dispose();
        }

        _playlist = new List<Media>();
        foreach (var path in paths)
        {
            _playlist.Add(new Media(_libvlc, path.FullPath));
        }

        _playlistIndex = 0;
        _mediaplayer = new MediaPlayer(_libvlc); 
        _mediaplayer.EndReached += PlayNextSiren;
        
        Console.WriteLine($"Playing Siren Display first song {_playlist.First()}");
        _mediaplayer.Play(_playlist[_playlistIndex]);
        ++_playlistIndex;
    }

    public void ToPlayIconTrue(object sender, EventArgs e)
    {
        // LibVLC events fire on a background thread. 
        // this is how we tell Avalonia to update the UI on the main thread
        Dispatcher.UIThread.Post(() => 
        {
            IsPlayButton = true;
        });
    }

    public async void PlayNextSiren(object sender, EventArgs e)
    {
        // Dispatch to UI thread to prevent cross-thread crashes
        Dispatcher.UIThread.Post(() => 
        {
            if (_playlistIndex < _playlist.Count)
            {
                // We don't call CleanupCurrentMedia here because we want to reuse the player for the playlist
                _mediaplayer.Play(_playlist[_playlistIndex]);
                ++_playlistIndex;
            }
            else
            {
                Stop();
            }
        });
    }

    public bool IsPlaying()
    {
        return _mediaplayer != null && _mediaplayer.IsPlaying;
    }
    
    public void Stop()
    {
        CleanupCurrentMedia();
        IsPlayButton = true;
        Console.WriteLine("we stopped the music player in audiocontroller");
    }
}