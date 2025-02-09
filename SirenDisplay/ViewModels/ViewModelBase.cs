using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Views;

namespace SirenDisplay.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    
    
    
    
    
    public void SwitchToClockView(InitViewModel currentViewref, ClockViewModel clockViewModelref )
    {
        currentViewref.CurrentView = clockViewModelref;
    }

    public void SwitchToAlarmView(InitViewModel currentViewref, AlarmViewModel alarmViewModelref )
    {
        currentViewref.CurrentView = alarmViewModelref;
    }
}