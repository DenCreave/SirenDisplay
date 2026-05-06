using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SirenDisplay.ViewModels;

namespace SirenDisplay.Views;

public partial class STCView : UserControl
{
    public STCView()
    {
        InitializeComponent();
    }

    private void StepMe(object? sender, PointerPressedEventArgs e)
    {
        var tmp = DataContext as STCViewModel;
        tmp.PostInit();
    }
}