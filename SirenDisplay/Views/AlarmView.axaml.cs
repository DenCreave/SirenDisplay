using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using HarfBuzzSharp;
using SirenDisplay.ViewModels;

namespace SirenDisplay.Views;

public partial class AlarmView : UserControl
{
    public AlarmView()
    {
        InitializeComponent();
    }


    private void SaveAndExit(object? sender, PointerPressedEventArgs e)
    {
        var tmp = this.DataContext as AlarmViewModel;
        tmp.SaveIATC();
        tmp.SwitchToClockView(tmp.CacheReferences.InitViewModel,tmp.CacheReferences.clockViewModel);
    }

    private void IncreaseHourDecimal(object? sender, PointerPressedEventArgs e)
    {
        var tmp = this.DataContext as AlarmViewModel;
        tmp.IncreaseHourDecimal();
    }

    private void DecreaseHourDecimal(object? sender, PointerPressedEventArgs e)
    {
        var tmp = this.DataContext as AlarmViewModel;
        tmp.DecreaseHourDecimal();
    }

    private void IncreaseHour(object? sender, PointerPressedEventArgs e)
    {
        var tmp = this.DataContext as AlarmViewModel;
        tmp.IncreaseHour();
    }
    private void DecreaseHour(object? sender, PointerPressedEventArgs e)
    {
        var tmp = this.DataContext as AlarmViewModel;
        tmp.DecreaseHour();
    }
    private void IncreaseMinuteDecimal(object? sender, PointerPressedEventArgs e)
    {
        var tmp = this.DataContext as AlarmViewModel;
        tmp.IncreaseMinuteDecimal();
    }
    private void DecreaseMinuteDecimal(object? sender, PointerPressedEventArgs e)
    {
        var tmp = this.DataContext as AlarmViewModel;
        tmp.DecreaseMinuteDecimal();
    }
    private void IncreaseMinute(object? sender, PointerPressedEventArgs e)
    {
        var tmp = this.DataContext as AlarmViewModel;
        tmp.IncreaseMinute();
    }
    private void DecreaseMinute(object? sender, PointerPressedEventArgs e)
    {
        var tmp = this.DataContext as AlarmViewModel;
        tmp.DecreaseMinute();
    }
}