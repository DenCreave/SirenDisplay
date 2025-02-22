using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibVLCSharp.Shared;
using SirenDisplay.Model;

namespace SirenDisplay.Controllers;

public sealed class AudioController
{
    private LibVLC _libvlc {get; set;}
    private Media _media { get; set; } //= new Media(_libvlc, new Uri(@"C:\tmp\big_buck_bunny.mp4"));
    private MediaPlayer _mediaplayer { get; set; }//= new MediaPlayer(_media);
    private List<DirectoryItem> _playlist { get; set; }
    public AudioController()
    {
        _libvlc=new LibVLC(enableDebugLogs: true);
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
    }

    public async Task PlaySirenDisplay(List<DirectoryItem> paths)
    {
        if (_mediaplayer.IsPlaying)
        {
            Stop();
        }
        _playlist = new List<DirectoryItem>(paths);
        _mediaplayer = new MediaPlayer(_libvlc);
        _mediaplayer.EndReached += PlayNextSiren;
        _media= new Media(_libvlc,_playlist.First().FullPath);
        Console.WriteLine($"Playing Siren Display first song {_playlist.First()}");
        _playlist.RemoveAt(0);
        _mediaplayer.Play(_media);
    }

    public void PlayNextSiren(object sender, EventArgs e)
    {
        if (_playlist.Count != 0)
        {
            Console.WriteLine($"Playing Siren Display another song {_playlist.First()}");
            _media = new Media(_libvlc, _playlist.First().FullPath);
            _mediaplayer.Play(_media);
            _playlist.RemoveAt(0);
        }
        else
        {
            Stop();
        }
    }

    public bool IsPlaying()
    {
        return _mediaplayer.IsPlaying;
    }
    public void Stop()
    {
        _mediaplayer.Dispose();
        Console.WriteLine("we stopped the music player in audo, quitting");
    }
}