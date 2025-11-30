using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class AuthViewModel : ViewModelBase
{
    public AuthViewModel(INavigationService navigationService) 
        : base(navigationService)
    {
    }

    [ObservableProperty] private int selectedTabIndex = 1;

    public ObservableCollection<string> Roles { get; } = new() { "Родитель", "Ребёнок" };
    [ObservableProperty] private string selectedRole = "";

    [ObservableProperty] private string registerLoginInput = "";
    [ObservableProperty] private string registerPasswordInput = "";
    [ObservableProperty] private string registerRepeatPasswordInput = "";

    [ObservableProperty] private string registerRoleError = "";
    [ObservableProperty] private string registerLoginInputError = "";
    [ObservableProperty] private string registerPasswordInputError = "";
    [ObservableProperty] private string registerRepeatPasswordInputError = "";


    [ObservableProperty] private string loginInput = "";
    [ObservableProperty] private string passwordInput = "";   

    [ObservableProperty] private string loginInputError = "";
    [ObservableProperty] private string passwordInputError = ""; 

    [RelayCommand]
    private void LoginUser()
    {
        ClearLoginErrors();

        bool isValid = true;

        if (string.IsNullOrWhiteSpace(LoginInput))
        {
            LoginInputError = "Введите email или телефон";
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(PasswordInput))
        {
        PasswordInputError = "Введите пароль";
            isValid = false;
        }

        if (!isValid) return;

        var scheduleVm = new ScheduleScreenViewModel(NavigationService);
        NavigationService.NavigateTo(scheduleVm);
    }

    [RelayCommand]
    private void RegisterUser()
    {
        ClearRegisterErrors();

        bool isValid = true;

        if (string.IsNullOrWhiteSpace(SelectedRole))
        {
            RegisterRoleError = "Выберите роль";
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(RegisterLoginInput))
        {
            RegisterLoginInputError = "Введите email или телефон";
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(RegisterPasswordInput))
        {
            RegisterPasswordInputError = "Введите пароль";
            isValid = false;
        }

        if (RegisterPasswordInput != RegisterRepeatPasswordInput)
        {
            RegisterRepeatPasswordInputError = "Пароли не совпадают";
            isValid = false;
        }

        if (!isValid) return;
        
        var scheduleVm = new ScheduleScreenViewModel(NavigationService);
        NavigationService.NavigateTo(scheduleVm);
    }

    private void ClearLoginErrors()
    {
        LoginInputError = "";
        PasswordInputError = "";
    }

    private void ClearRegisterErrors()
    {
        RegisterRoleError = "";
        RegisterLoginInputError = "";
        RegisterPasswordInputError = "";
        RegisterRepeatPasswordInputError = "";
    }
}
