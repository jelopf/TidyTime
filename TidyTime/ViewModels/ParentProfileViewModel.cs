using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class ParentProfileViewModel : ViewModelBase
{
    private readonly IAuthService _authService;

    public ParentProfileViewModel(INavigationService navigationService, IAuthService authService) 
        : base(navigationService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private void GoBack()
    {
        NavigationService.NavigateTo(new ScheduleScreenViewModel(NavigationService, _authService));
    }

    [RelayCommand]
    private void GoToAuth()
    {
        NavigationService.NavigateTo(new AuthViewModel(NavigationService, _authService));
    }
}
