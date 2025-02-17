using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Assets.Polygons.Frames;
using SirenDisplay.Model;

namespace SirenDisplay.ViewModels;

public sealed partial class MusicViewModel : ViewModelBase
{
    [ObservableProperty] private Path _mainFrame;
    public LabelData LabelData { get; set; }
    public CacheReferences CacheReferences { get; set; }

    public MusicViewModel()
    {
        FrameInitializer();
    }
    private void FrameInitializer()
    {
        MainFrame = new Path
        {
            Stroke =  Application.Current.FindResource("OffColor") as LinearGradientBrush,
            StrokeThickness = 10,
            Stretch = Stretch.Fill,
            Data = new PathGeometry
            {
                Figures = new PathFigures
                {
                    new TopFrameSquare().PathFigure,
                    new MiddleFrame().PathFigure,
                    new BottomFrameSquare().PathFigure
                }
            },
            Effect = Application.Current.FindResource("OffEffect") as DropShadowEffect
        };
    }
}