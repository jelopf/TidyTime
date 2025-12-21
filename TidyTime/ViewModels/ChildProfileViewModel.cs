using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Models;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class ChildProfileViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;
    private User? _currentUser;

    [ObservableProperty]
    private string _userName = "";

    [ObservableProperty]
    private string _userFullName = "";

    [ObservableProperty]
    private int _totalCoinsDisplay;

    [ObservableProperty]
    private int _completedTasksCountDisplay;

    [ObservableProperty]
    private bool _isLoading = true;

    public ChildProfileViewModel(INavigationService navigationService, IAuthService authService, ITaskService taskService) 
        : base(navigationService)
    {
        _authService = authService;
        _taskService = taskService;
        _currentUser = authService.GetCurrentUser();

        if (_currentUser != null)
        {
            UserName = _currentUser.Login;
            UserFullName = _currentUser.FullName;
        }
        
        LoadProfileDataAsync();
    }

    private void LoadProfileDataAsync()
    {
        if (_currentUser == null) return;
        
        try
        {
            IsLoading = true;
            
            TotalCoinsDisplay = _currentUser.TotalCoins;
            CompletedTasksCountDisplay = _currentUser.CompletedTasksCount;
        }
        finally
        {
            IsLoading = false;
        }
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
