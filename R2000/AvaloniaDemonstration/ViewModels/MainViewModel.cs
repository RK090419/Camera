using CommunityToolkit.Mvvm.Input;
using Core.Interfaces;

namespace AvaloniaDemonstration.ViewModels;

public sealed partial class MainViewModel : ViewModelBase
{
    readonly INavigationService _navigationService;
    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    [RelayCommand]
    void Next()
    {
        _navigationService.Navigate<MainView, CameraView>();
    }
}
