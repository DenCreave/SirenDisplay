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
}