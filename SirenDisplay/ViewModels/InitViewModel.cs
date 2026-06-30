using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Assets.SpanningTree.Controller;
using SirenDisplay.Assets.SpanningTree.UI;
using SirenDisplay.Controllers;
using SirenDisplay.Model;
using SirenDisplay.Views;

namespace SirenDisplay.ViewModels;

public sealed partial class InitViewModel : ViewModelBase
{
    public SpanningTreeController Stc { get; }
    public SpanningTreeTheme Stt { get; } 
    [ObservableProperty] private ViewModelBase _currentView;
    private CacheReferences _cacheReferences;
    private AlarmTimerController _alarmTimerController; 
    
    public InitViewModel(SpanningTreeController stc, SpanningTreeTheme stt)
    {
        #region inits
        _alarmTimerController = AlarmTimerController.Instance;
        
        _cacheReferences = new CacheReferences()
        {
            InitViewModel = this,
            alarmTimerController = _alarmTimerController
        };
        #endregion inits

        Stc = stc;
        Stt = stt;

        //SwitchToClockView(_cacheReferences);
        //SwitchToSTCView(_cacheReferences);
    }
    
}