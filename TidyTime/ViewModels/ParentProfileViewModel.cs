using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Models;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class ParentProfileViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;
    private User? _currentUser;

    [ObservableProperty]
    private string _userName = "";

    [ObservableProperty]
    private string _userFullName = "";

    [ObservableProperty]
    private ObservableCollection<User> _children = new();

    [ObservableProperty]
    private bool _isLoading = true;

    public ParentProfileViewModel(INavigationService navigationService, IAuthService authService, ITaskService taskService) 
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
        
        _ = LoadChildrenAsync();
    }

    private async Task LoadChildrenAsync()
    {
        if (_currentUser == null) return;
        
        try
        {
            IsLoading = true;
            
            var children = await _taskService.GetChildrenForParentAsync(_currentUser.Id);
            
            Children.Clear();
            foreach (var child in children)
            {
                Children.Add(child);
            }
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

    [RelayCommand]
    private async Task AddChild()
    {
        // TODO: Реализовать добавление ребенка
        await LoadChildrenAsync();
    }
}
