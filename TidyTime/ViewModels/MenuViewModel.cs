using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class MenuViewModel : ViewModelBase
{
    public MenuViewModel(INavigationService navigationService)
        : base(navigationService)
    {
    }

    [RelayCommand]
    private void GoBack()
    {
        var vm = new ScheduleScreenViewModel(NavigationService);
        NavigationService.NavigateTo(vm);
    }
}
