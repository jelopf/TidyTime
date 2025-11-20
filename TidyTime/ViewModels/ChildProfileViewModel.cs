using CommunityToolkit.Mvvm.ComponentModel;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class ChildProfileViewModel : ViewModelBase
{
    public ChildProfileViewModel(INavigationService navigationService) 
        : base(navigationService)
    {
    }
}