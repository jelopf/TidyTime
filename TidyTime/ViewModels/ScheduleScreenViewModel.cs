using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class ScheduleScreenViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool isPopupOpen;

    public AddTaskPopupViewModel AddTaskPopupViewModel { get; }

    public ScheduleScreenViewModel(INavigationService navigationService) 
        : base(navigationService)
    {
        AddTaskPopupViewModel = new AddTaskPopupViewModel(() => CloseAddTask());
    }

    [RelayCommand]
    private void GoToProfile(string role)
    {
        if (role == "Child")
        {
            var vm = new ChildProfileViewModel(NavigationService);
            NavigationService.NavigateTo(vm);
        }
        else
        {
            var vm = new ParentProfileViewModel(NavigationService);
            NavigationService.NavigateTo(vm);
        }
    }

    [RelayCommand]
    private void GoToMenu()
    {
        var vm = new MenuViewModel(NavigationService);
        NavigationService.NavigateTo(vm);
    }

    [RelayCommand]
    private void OpenAddTask()
    {
        IsPopupOpen = true;
    }

    private void CloseAddTask()
    {
        IsPopupOpen = false;
    }
}
