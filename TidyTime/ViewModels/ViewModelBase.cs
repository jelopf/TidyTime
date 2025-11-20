using CommunityToolkit.Mvvm.ComponentModel;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    protected INavigationService NavigationService { get; }

    protected ViewModelBase(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }
}
