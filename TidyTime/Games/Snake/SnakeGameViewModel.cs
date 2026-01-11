using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;
using TidyTime.Models;
using TidyTime.ViewModels;

namespace TidyTime.Games.Snake;

public partial class SnakeGameViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;
    private User? _currentUser;
    
    [ObservableProperty]
    private bool _gameStarted;
    
    public SnakeGameViewModel(INavigationService navigationService, 
                             IAuthService authService,
                             ITaskService taskService)
        : base(navigationService)
    {
        _authService = authService;
        _taskService = taskService;
        _currentUser = _authService.GetCurrentUser();
        GameStarted = true;
    }
    
    [RelayCommand]
    private void GoBack()
    {
        var vm = new GameMenuViewModel(NavigationService, _authService, _taskService);
        NavigationService.NavigateTo(vm);
    }
}