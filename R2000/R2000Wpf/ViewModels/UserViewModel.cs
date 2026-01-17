using CommunityToolkit.Mvvm.Input;
using Core.Interfaces;
using R2000Wpf.Models;
using R2000Wpf.Views;

namespace R2000Wpf.ViewModels;

public sealed partial class UserViewModel : ViewModelBase
{
    readonly INavigationService _navigationService;
    public UserViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    [RelayCommand]
    void Back()
    {
        _navigationService.Navigate<UserView, LoginView>();
    }
}
