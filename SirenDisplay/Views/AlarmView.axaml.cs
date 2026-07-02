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
        if (this.DataContext is AlarmViewModel tmp)
        {
            tmp.SaveIATC();
            tmp.Navigator.NavigateTo<ClockViewModel>();
        }
    }

    private void IncreaseHourDecimal(object? sender, PointerPressedEventArgs e)
    {
        if (this.DataContext is AlarmViewModel tmp) tmp.IncreaseHourDecimal();
    }

    private void DecreaseHourDecimal(object? sender, PointerPressedEventArgs e)
    {
        if (this.DataContext is AlarmViewModel tmp) tmp.DecreaseHourDecimal();
    }

    private void IncreaseHour(object? sender, PointerPressedEventArgs e)
    {
        if (this.DataContext is AlarmViewModel tmp) tmp.IncreaseHour();
    }

    private void DecreaseHour(object? sender, PointerPressedEventArgs e)
    {
        if (this.DataContext is AlarmViewModel tmp) tmp.DecreaseHour();
    }

    private void IncreaseMinuteDecimal(object? sender, PointerPressedEventArgs e)
    {
        if (this.DataContext is AlarmViewModel tmp) tmp.IncreaseMinuteDecimal();
    }

    private void DecreaseMinuteDecimal(object? sender, PointerPressedEventArgs e)
    {
        if (this.DataContext is AlarmViewModel tmp) tmp.DecreaseMinuteDecimal();
    }

    private void IncreaseMinute(object? sender, PointerPressedEventArgs e)
    {
        if (this.DataContext is AlarmViewModel tmp) tmp.IncreaseMinute();
    }

    private void DecreaseMinute(object? sender, PointerPressedEventArgs e)
    {
        if (this.DataContext is AlarmViewModel tmp) tmp.DecreaseMinute();
    }
}