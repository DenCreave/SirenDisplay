using System;
using CommunityToolkit.Mvvm.ComponentModel;
using HarfBuzzSharp;
using SirenDisplay.Model;
using SirenDisplay.Views;

namespace SirenDisplay.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    
    public void SwitchToClockView(CacheReferences references)
    {
        references.InitViewModel.CurrentView= new ClockViewModel()
        {
            CacheReferences = references
        };
        Console.WriteLine("SwitchToClockView");
    }
    

     public void SwitchToAlarmView(CacheReferences references )
     {
         references.InitViewModel.CurrentView = new AlarmViewModel()
         {
             CacheReferences = references
         };
         var tmp = references.InitViewModel.CurrentView as AlarmViewModel;
         tmp.LoadIATC();
     }
    /*public void SwitchToClockView(InitViewModel currentViewref, ClockViewModel clockViewModelref )
    {
        currentViewref.CurrentView = clockViewModelref;
    }*/

   /* public void SwitchToAlarmView(InitViewModel currentViewref, AlarmViewModel alarmViewModelref )
    {
        currentViewref.CurrentView = alarmViewModelref;
    }*/
}