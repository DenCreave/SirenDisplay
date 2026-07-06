using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Controllers;
using SirenDisplay.Model;
using SirenDisplay.Services;
using SirenDisplay.SpanningTree.Controller;
using SirenDisplay.SpanningTree.UI;
using SirenDisplay.Views;

namespace SirenDisplay.ViewModels;

public sealed partial class InitViewModel : ViewModelBase
{
    public SpanningTreeController Stc { get; }
    public SpanningTreeTheme Stt { get; }
    public NavigationService Navigator { get; }
    
    public InitViewModel(
        SpanningTreeController stc,
        SpanningTreeTheme stt,
        NavigationService navigator)
    {
        Stc = stc;
        Stt = stt;
        Navigator = navigator;
        
        Navigator.NavigateTo<ClockViewModel>();
    }
    
}