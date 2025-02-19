using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SirenDisplay.ViewModels;

namespace SirenDisplay.Views;

public partial class MusicView : UserControl
{
    public MusicView()
    {
        InitializeComponent();
    }

    public void DirectoryUp(object? sender, PointerPressedEventArgs e)
    {
        var tmp = DataContext as MusicViewModel;
        tmp.DirectoryUp();
    }

    public void DirectoryDown(object? sender, PointerPressedEventArgs e)
    {
        var tmp = DataContext as MusicViewModel;
        tmp.DirectoryDown();
    }

    public void MusicPathUp(object? sender, PointerPressedEventArgs e)
    {
        var tmp = DataContext as MusicViewModel;
        tmp.MusicPathUp();
    }

    public void MusicPathDown(object? sender, PointerPressedEventArgs e)
    {
        var tmp = DataContext as MusicViewModel;
        tmp.MusicPathDown();
    }

    public void SaveAndExit(object? sender, PointerPressedEventArgs e)
    {
        var tmp = DataContext as MusicViewModel;
        tmp.SaveAndExit();
    }

    public void AddToPlaylist(object? sender, PointerPressedEventArgs e)
    {
        var tmp = DataContext as MusicViewModel;
        tmp.AddToPlaylist();
    }

    public void RemoveFromPlaylist(object? sender, PointerPressedEventArgs e)
    {
        var tmp = DataContext as MusicViewModel;
        tmp.RemoveFromPlaylist();
    }

    public void PlayStopMedia(object? sender, PointerPressedEventArgs e)
    {
        var tmp = DataContext as MusicViewModel;
        tmp.PlayStopMedia();
    }
    
}