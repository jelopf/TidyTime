using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class ChildProfileViewModel : ViewModelBase
{
    public ChildProfileViewModel(INavigationService navigationService) 
        : base(navigationService)
    {
    }

    [RelayCommand]
    private void GoBack()
    {
        // Возврат на экран расписания
        NavigationService.NavigateTo(new ScheduleScreenViewModel(NavigationService));
    }

    [RelayCommand]
    private void GoToAuth()
    {
        NavigationService.NavigateTo(new AuthViewModel(NavigationService));
    }

}
