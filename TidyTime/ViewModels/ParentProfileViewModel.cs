using CommunityToolkit.Mvvm.ComponentModel;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class ParentProfileViewModel : ViewModelBase
{
    public ParentProfileViewModel(INavigationService navigationService) 
        : base(navigationService)
    {
    }
}