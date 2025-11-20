using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class AuthViewModel : ViewModelBase
{
    public AuthViewModel(INavigationService navigationService) 
        : base(navigationService)
    {
    }

    [ObservableProperty] private int selectedTabIndex = 1;

    [ObservableProperty] private string selectedRole = "";
    public ObservableCollection<string> Roles { get; } = new() { "Родитель", "Ребёнок" };

    [ObservableProperty] private string login = "";
    [ObservableProperty] private string password = "";
    [ObservableProperty] private string repeatPassword = "";

    [RelayCommand]
    private void LoginUser()
    {
        var view = new ScheduleScreenView();
        view.DataContext = new ScheduleScreenViewModel(NavigationService);
        NavigationService.NavigateTo(view);
    }

    [RelayCommand]
    private void RegisterUser()
    {
        var view = new ScheduleScreenView();
        view.DataContext = new ScheduleScreenViewModel(NavigationService);
        NavigationService.NavigateTo(view);
    }
}
