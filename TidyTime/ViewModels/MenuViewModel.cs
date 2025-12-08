using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class MenuViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;

    public MenuViewModel(INavigationService navigationService, IAuthService authService, ITaskService taskService)
        : base(navigationService)
    {
        _authService = authService;
        _taskService = taskService;
    }

    [RelayCommand]
    private void GoBack()
    {
        IDayOfWeekService dayOfWeekService = new DayOfWeekService();
        var vm = new ScheduleScreenViewModel(NavigationService, _authService, _taskService, dayOfWeekService);
        NavigationService.NavigateTo(vm);
    }
}
