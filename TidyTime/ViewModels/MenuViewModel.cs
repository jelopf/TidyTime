using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class MenuViewModel : ViewModelBase
{
    private readonly IAuthService _authService;

    public MenuViewModel(INavigationService navigationService, IAuthService authService)
        : base(navigationService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private void GoBack()
    {
        var vm = new ScheduleScreenViewModel(NavigationService, _authService);
        NavigationService.NavigateTo(vm);
    }
}
