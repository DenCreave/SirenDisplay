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
        if (DataContext is MusicViewModel tmp) tmp.DirectoryUp();
    }

    public void DirectoryDown(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MusicViewModel tmp) tmp.DirectoryDown();
    }

    public void MusicPathUp(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MusicViewModel tmp) tmp.MusicPathUp();
    }

    public void MusicPathDown(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MusicViewModel tmp) tmp.MusicPathDown();
    }

    public void MusicOrderUp(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MusicViewModel tmp) tmp.MusicOrderUp();
    }

    public void MusicOrderDown(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MusicViewModel tmp) tmp.MusicOrderDown();
    }

    public void SaveAndExit(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MusicViewModel tmp) tmp.SaveAndExit();
    }

    public void AddToPlaylist(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MusicViewModel tmp) tmp.AddToPlaylist();
    }

    public void RemoveFromPlaylist(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MusicViewModel tmp) tmp.RemoveFromPlaylist();
    }

    public void PlayStopMedia(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MusicViewModel tmp) tmp.PlayStopMedia();
    }

    private void UpADir(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MusicViewModel tmp) tmp.UpADir();
    }
}