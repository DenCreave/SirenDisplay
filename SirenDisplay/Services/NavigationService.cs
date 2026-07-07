using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.ViewModels;

namespace SirenDisplay.Services;

public partial class NavigationService : ObservableObject
{
    [ObservableProperty] 
    private ViewModelBase _currentView;
    private readonly IServiceProvider _services;

    // We inject IServiceProvider here so the NavigationService can ask DI to build the ViewModels!
    public NavigationService(IServiceProvider services)
    {
        _services = services;
    }

    // Generic method to switch views cleanly
    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        if (CurrentView is IDisposable disposableView)
        {
            disposableView.Dispose();
        }
        
        // Ask DI to build the ViewModel (which automatically injects its dependencies!)
        CurrentView = (ViewModelBase)_services.GetService(typeof(TViewModel));
    }
}