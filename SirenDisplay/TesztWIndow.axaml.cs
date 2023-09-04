using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SirenDisplay;

public partial class TesztWIndow : Window
{
    public TesztWIndow()
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