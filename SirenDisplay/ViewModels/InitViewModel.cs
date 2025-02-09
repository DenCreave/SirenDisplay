using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SirenDisplay.Model;

namespace SirenDisplay.ViewModels;

public sealed partial class InitViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentView;
    private ClockViewModel _clockViewModel;
    private AlarmViewModel _alarmViewModel;
    private CacheVM _cacheVM;
    public InitViewModel()
    {
        _clockViewModel = new ClockViewModel();
        _alarmViewModel = new AlarmViewModel();

        _cacheVM = new CacheVM()
        {
            InitViewModel = this,
            clockViewModel = _clockViewModel,
            alarmViewModel = _alarmViewModel
        };
        
        _clockViewModel.CacheVM = _cacheVM;
        _alarmViewModel.CacheVM = _cacheVM;
        
        SwitchToClockView(this, _clockViewModel);
    }
    
}