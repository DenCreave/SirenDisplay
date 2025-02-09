using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Assets.Polygons.Frames;
using SirenDisplay.Classes.Digits;
using SirenDisplay.Model;

namespace SirenDisplay.ViewModels;

public sealed partial class AlarmViewModel : ViewModelBase
{
    public CacheVM CacheVM { get; set; }
    
}