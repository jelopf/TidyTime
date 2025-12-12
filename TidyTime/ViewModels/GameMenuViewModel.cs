using CommunityToolkit.Mvvm.ComponentModel;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class GameMenuViewModel : ViewModelBase
{
    public GameMenuViewModel(INavigationService navigationService) : base(navigationService)
    {
    }

    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";
}