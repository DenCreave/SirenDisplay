using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SirenDisplay.ViewModels;

namespace SirenDisplay.Views;

public partial class InitView : Window
{
    public InitView()
    {
        InitializeComponent();
        DataContext = new InitViewModel();
        WindowState = WindowState.FullScreen;
    }
}