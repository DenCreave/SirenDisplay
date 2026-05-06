using System;
using CommunityToolkit.Mvvm.ComponentModel;
using HarfBuzzSharp;
using SirenDisplay.Assets.SpanningTree.DotMap;
using SirenDisplay.Controllers;
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
        var tmp = references.InitViewModel.CurrentView as ClockViewModel;
        tmp.PostLoad();
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

     public void SwitchToMusicView(CacheReferences references)
     {
         references.InitViewModel.CurrentView = new MusicViewModel()
         {
             CacheReferences = references
         };
         var tmp = references.InitViewModel.CurrentView as MusicViewModel;
         tmp.PostInit();
     }

     public void SwitchToSTCView(CacheReferences references)
     {
         references.InitViewModel.CurrentView = new STCViewModel(); { };
         
     }
}