using CommunityToolkit.Mvvm.Input;
using Core.Interfaces;
using R2000Wpf.Models;
using R2000Wpf.Views;

namespace R2000Wpf.ViewModels;

public sealed partial class LoginViewModel : ViewModelBase
{
    readonly INavigationService _navigationService;
    public LoginViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    void LogIn()
    {
        _navigationService.Navigate<LoginView, UserView>();
    }
}
