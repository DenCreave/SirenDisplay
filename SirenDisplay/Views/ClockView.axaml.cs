using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using SirenDisplay.ViewModels;

namespace SirenDisplay;

public partial class ClockView : Window
{
    public ClockView()
    {
        InitializeComponent();
        //DataContext = new ClockViewModel();
    }

    private void ToggleAlarm(object? sender, PointerPressedEventArgs e)
    {
        //var tmp = DataContext as ClockViewModel;
        (DataContext as ClockViewModel)?.ActivateAlarmButton();
    }
}