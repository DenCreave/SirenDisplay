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
    private readonly LibVLC _libvlc;
    private readonly MediaPlayer _mediaplayer;
    
    private Media? _currentMedia; 
    private LinkedList<string>? _playlist;
    private LinkedListNode<string>? _currentTrackPath;
    private bool _isPlaylistMode;

    public LabelData LabelData { get; }
    public string PlayButton => IsPlayButton ? LabelData.PlayLabel : LabelData.StopLabel;

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(PlayButton))] 
    private bool _isPlayButton;

    public AudioController()
    {
        _libvlc = new LibVLC(enableDebugLogs: false);
        _mediaplayer = new MediaPlayer(_libvlc);
        
        LabelData = new LabelData();
        IsPlayButton = true;
        
        _mediaplayer.EndReached += OnMediaPlayerEndReached;
    }


    private void DisposeCurrentMedia()
    {
        if (_currentMedia != null)
        {
            _currentMedia.Dispose();
            _currentMedia = null;
        }
    }

    public async Task PlayAudio(string path)
    {
        _mediaplayer.Stop();
        DisposeCurrentMedia();
        _isPlaylistMode = false;
        
        _currentMedia = new Media(_libvlc, path);
        _mediaplayer.Play(_currentMedia);
        IsPlayButton = false;
    }

    public async Task PlaySirenDisplay(List<DirectoryItem> paths)
    {
        if (paths == null || paths.Count == 0) return;
        
        _mediaplayer.Stop();
        DisposeCurrentMedia();

        _isPlaylistMode = true;
        _playlist = new LinkedList<string>(paths.Select(x => x.FullPath));
        _currentTrackPath = _playlist.First;

        if (_currentTrackPath != null)
        {
            Console.WriteLine($"Playing Siren Display first song: {_currentTrackPath.Value}");
            
            _currentMedia = new Media(_libvlc, _currentTrackPath.Value);
            _mediaplayer.Play(_currentMedia);
            
            IsPlayButton = false;
        }
    }

    
    private void OnMediaPlayerEndReached(object? sender, EventArgs e)
    {
        // LibVLC events fire on a background thread. 
        // We MUST tell Avalonia to update the UI on the main thread!
        Dispatcher.UIThread.Post(() => 
        {
            if (_isPlaylistMode && _currentTrackPath?.Next != null)
            {
                _currentTrackPath = _currentTrackPath.Next;
                DisposeCurrentMedia(); 
                
                Console.WriteLine($"Next song: {_currentTrackPath.Value}");
                _currentMedia = new Media(_libvlc, _currentTrackPath.Value);
                _mediaplayer.Play(_currentMedia);
            }
            else
            {
                Stop();
            }
        });
    }

    public bool IsPlaying() => _mediaplayer.IsPlaying;
    
    public void Stop()
    {
        _mediaplayer.Stop();
        IsPlayButton = true;
        _isPlaylistMode = false;
        Console.WriteLine("Music Player Stopped.");
    }
}