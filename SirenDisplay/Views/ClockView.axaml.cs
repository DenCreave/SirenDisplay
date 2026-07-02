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
        RestartSTC();
        if (DataContext is ClockViewModel tmp) tmp.ActivateAlarmButton();
    }

    private void SetAlarm(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ClockViewModel tmp) tmp.Navigator.NavigateTo<AlarmViewModel>();
    }

    private void SetPlayList(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is ClockViewModel tmp) tmp.Navigator.NavigateTo<MusicViewModel>();
    }
    
    private void RestartSTC()
    {
        if (DataContext is ClockViewModel tmp) tmp.RestartSTC();
    }
}