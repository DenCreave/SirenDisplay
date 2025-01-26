using Avalonia.Controls;
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