using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SirenDisplay.ViewModels;

namespace SirenDisplay.Views;

public partial class ClockView : UserControl
{
    public ClockView()
    {
        InitializeComponent();
    }
    private void ToggleAlarm(object? sender, PointerPressedEventArgs e)
    {
        (DataContext as ClockViewModel)?.ActivateAlarmButton();
    }

    private void SetAlarm(object? sender, RoutedEventArgs e)
    {
        var tmp = DataContext as ClockViewModel;
        tmp.SwitchToAlarmView(tmp.CacheReferences);
    }

    private void SetPlayList(object? sender, PointerPressedEventArgs e)
    {
        var tmp = DataContext as ClockViewModel;
        tmp.SwitchToMusicView(tmp.CacheReferences);
    }
}