using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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
        //Cursor = new Cursor(StandardCursorType.None); //comment this line if you want to see the cursor
    }
}