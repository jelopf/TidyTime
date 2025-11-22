using CommunityToolkit.Mvvm.ComponentModel;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel(INavigationService navigationService) 
        : base(navigationService)
    {
    }

    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";
}
