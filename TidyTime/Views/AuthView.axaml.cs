using Avalonia.Controls;
using TidyTime.ViewModels;
using TidyTime.Services;

namespace TidyTime.Views;

public partial class AuthView : UserControl
{
    public AuthView(INavigationService navigationService)
    {
        InitializeComponent();
        DataContext = new AuthViewModel(navigationService);
    }
}
