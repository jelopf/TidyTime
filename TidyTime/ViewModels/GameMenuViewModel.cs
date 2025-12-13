using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Models;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class GameMenuViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;

    public GameMenuViewModel(INavigationService navigationService, IAuthService authService, ITaskService taskService) : base(navigationService)
    {
        _authService = authService;
        _taskService = taskService;
    }

    [RelayCommand]
    private void GoBack()
    {
        var vm = new MenuViewModel(NavigationService, _authService, _taskService);
        NavigationService.NavigateTo(vm);
    }
}