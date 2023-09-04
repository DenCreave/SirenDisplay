using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SirenDisplay;

public partial class AlarmWindow : Window
{
    public AlarmWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}