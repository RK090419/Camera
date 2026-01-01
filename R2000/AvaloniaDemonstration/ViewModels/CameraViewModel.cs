using CommunityToolkit.Mvvm.Input;
using Core.Interfaces;

namespace AvaloniaDemonstration.ViewModels;

public sealed partial class CameraViewModel : ViewModelBase
{
    readonly INavigationService _navigationService;
    public CameraViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    [RelayCommand]
    void Back()
    {
        _navigationService.Navigate<CameraView, MainView>();
    }
}

