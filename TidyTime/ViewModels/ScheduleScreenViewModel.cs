using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class ScheduleScreenViewModel : ViewModelBase
{
    public ScheduleScreenViewModel(INavigationService navigationService) 
        : base(navigationService)
    {
    }

    [RelayCommand]
    private void GoToProfile(string role)
    {
        if (role == "Child")
        {
            var view = new ChildProfileView();
            view.DataContext = new ChildProfileViewModel(NavigationService);
            NavigationService.NavigateTo(view);
        }
        else
        {
            var view = new ParentProfileView();
            view.DataContext = new ParentProfileViewModel(NavigationService);
            NavigationService.NavigateTo(view);
        }
    }
}
