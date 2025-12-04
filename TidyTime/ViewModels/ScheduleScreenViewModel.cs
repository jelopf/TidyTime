using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class ScheduleScreenViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;

    [ObservableProperty]
    private bool isPopupOpen;

    public AddTaskPopupViewModel? AddTaskPopupViewModel { get; }

    public ScheduleScreenViewModel(INavigationService navigationService, IAuthService authService, ITaskService taskService) 
        : base(navigationService)
    {
        _authService = authService;
        _taskService = taskService;

        var currentUser = _authService.GetCurrentUser();
        
        if (currentUser != null)
        {
            AddTaskPopupViewModel = new AddTaskPopupViewModel(
                _taskService,
                CloseAddTask,
                currentUser.Id
            );
        }
    }

    [RelayCommand]
    private void GoToProfile(string role)
    {
        if (role == "Child")
        {
            var vm = new ChildProfileViewModel(NavigationService, _authService, _taskService);
            NavigationService.NavigateTo(vm);
        }
        else
        {
            var vm = new ParentProfileViewModel(NavigationService, _authService, _taskService);
            NavigationService.NavigateTo(vm);
        }
    }

    [RelayCommand]
    private void GoToMenu()
    {
        var vm = new MenuViewModel(NavigationService, _authService, _taskService);
        NavigationService.NavigateTo(vm);
    }

    [RelayCommand]
    private void OpenAddTask()
    {
        IsPopupOpen = true;
    }

    private void CloseAddTask()
    {
        IsPopupOpen = false;
    }
}
