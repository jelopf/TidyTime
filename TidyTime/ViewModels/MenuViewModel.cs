using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Models;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class MenuViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;

    [ObservableProperty]
    private bool isParentMode = false;

    public MenuViewModel(INavigationService navigationService, IAuthService authService, ITaskService taskService)
        : base(navigationService)
    {
        _authService = authService;
        _taskService = taskService;

        var currentUser = _authService.GetCurrentUser();
        IsParentMode = currentUser?.Role == UserRole.Parent;
    }

    [RelayCommand]
    private void GoBack()
    {
        IDayOfWeekService dayOfWeekService = new DayOfWeekService();
        var vm = new ScheduleScreenViewModel(NavigationService, _authService, _taskService, dayOfWeekService);
        NavigationService.NavigateTo(vm);
    }

    [RelayCommand]
    private void GoToGames()
    {
        var vm = new GameMenuViewModel(NavigationService, _authService, _taskService);
        NavigationService.NavigateTo(vm);
    }
}
