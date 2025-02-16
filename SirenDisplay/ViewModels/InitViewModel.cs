using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Controllers;
using SirenDisplay.Model;
using SirenDisplay.Views;

namespace SirenDisplay.ViewModels;

public sealed partial class InitViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentView;
    private CacheReferences _cacheReferences;
    //private ClockViewModel _clockViewModel;
    //private AlarmViewModel _alarmViewModel;
    private AlarmTimerController _alarmTimerController; //maybe rename it to service, and start it from here
    
    
    
    public InitViewModel()
    {
        #region inits
        //_clockViewModel = new ClockViewModel();
        //_alarmViewModel = new AlarmViewModel();
        _alarmTimerController = new AlarmTimerController();
        
        _cacheReferences = new CacheReferences()
        {
            InitViewModel = this,
            /*clockViewModel = _clockViewModel,
            alarmViewModel = _alarmViewModel,*/
            alarmTimerController = _alarmTimerController
        };
        
        /*_clockViewModel.CacheReferences = _cacheReferences;
        _alarmViewModel.CacheReferences = _cacheReferences;*/
        #endregion inits
        
        #region post_init_settings
        //_alarmViewModel.LoadIATC();
        
        #endregion post_init_settings

        SwitchToClockView(_cacheReferences);
    }
    
}