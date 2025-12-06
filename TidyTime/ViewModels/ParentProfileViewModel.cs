using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class ParentProfileViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;

    public ParentProfileViewModel(INavigationService navigationService, IAuthService authService, ITaskService taskService) 
        : base(navigationService)
    {
        _authService = authService;
        _taskService = taskService;
    }

    [RelayCommand]
    private void GoBack()
    {
        IDayOfWeekService dayOfWeekService = new DayOfWeekService();
        NavigationService.NavigateTo(new ScheduleScreenViewModel(NavigationService, _authService, _taskService, dayOfWeekService));
    }

    [RelayCommand]
    private void GoToAuth()
    {
        NavigationService.NavigateTo(new AuthViewModel(NavigationService, _authService, _taskService));
    }
}
