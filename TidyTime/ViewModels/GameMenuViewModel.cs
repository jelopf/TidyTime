using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
}