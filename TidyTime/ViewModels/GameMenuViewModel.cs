using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Games.Memory;
using TidyTime.Games.Snake;
using TidyTime.Models;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class GameMenuViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;

    private User? _currentUser;

    [ObservableProperty]
    private int _totalCoinsDisplay = 0;
    
    public GameMenuViewModel(INavigationService navigationService, IAuthService authService, ITaskService taskService) : base(navigationService)
    {
        _authService = authService;
        _taskService = taskService;

        _currentUser = _authService.GetCurrentUser();

        LoadTotalCoins();
    }

    [RelayCommand]
    private void GoBack()
    {
        var vm = new MenuViewModel(NavigationService, _authService, _taskService);
        NavigationService.NavigateTo(vm);
    }

    private void LoadTotalCoins()
    {
        if (_currentUser?.Role == UserRole.Child)
            TotalCoinsDisplay = _currentUser.TotalCoins;
        else
            TotalCoinsDisplay = 0;
    }

    [RelayCommand]
    private Task PlaySnakeAsync()
    {
        if (_currentUser == null || _currentUser.TotalCoins < 20)
            return Task.CompletedTask; // безопасный досрочный выход

        _currentUser.TotalCoins -= 20;
        TotalCoinsDisplay = _currentUser.TotalCoins;

        return UpdateAndNavigateToSnakeAsync();
    }

    private async Task UpdateAndNavigateToSnakeAsync()
    {
        if (_currentUser != null)
            await _authService.UpdateUserAsync(_currentUser); // обновляем Firebase

        NavigationService.NavigateTo(
            new SnakeGameViewModel(NavigationService, _authService, _taskService)
        );
    }
    
    [RelayCommand]
    private Task PlayMemoryAsync()
    {
        if (_currentUser == null || _currentUser.TotalCoins < 20)
            return Task.CompletedTask;

        _currentUser.TotalCoins -= 20;
        TotalCoinsDisplay = _currentUser.TotalCoins;

        return UpdateAndNavigateToReactionAsync();
    }

    private async Task UpdateAndNavigateToReactionAsync()
    {
        if (_currentUser != null)
            await _authService.UpdateUserAsync(_currentUser);

        NavigationService.NavigateTo(
            new MemoryGameViewModel(NavigationService, _authService, _taskService)
        );
    }
}