using Avalonia.Controls;
using Avalonia.Interactivity;
using SirenDisplay.Classes;
using SirenDisplay.ViewModels;

namespace SirenDisplay;

public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        DataContext = new MainViewModel();
        InitializeComponent();
    }
    

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        MusicWindow win2 = new MusicWindow();

        this.Content = win2.Content;
    }
}