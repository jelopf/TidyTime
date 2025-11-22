using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class ParentProfileViewModel : ViewModelBase
{
    public ParentProfileViewModel(INavigationService navigationService) 
        : base(navigationService)
    {
    }

    [RelayCommand]
    private void GoBack()
    {
        NavigationService.NavigateTo(new ScheduleScreenViewModel(NavigationService));
    }

    [RelayCommand]
    private void GoToAuth()
    {
        NavigationService.NavigateTo(new AuthViewModel(NavigationService));
    }
}
