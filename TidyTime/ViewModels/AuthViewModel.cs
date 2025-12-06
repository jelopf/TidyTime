using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TidyTime.Services;
using TidyTime.Views;
using TidyTime.Models;

namespace TidyTime.ViewModels;

public partial class AuthViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;

    public AuthViewModel(INavigationService navigationService, IAuthService authService, ITaskService taskService) 
        : base(navigationService)
    {
        _authService = authService;
        _taskService = taskService;
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
    private async Task LoginUser()
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

        var user = await _authService.LoginUserAsync(LoginInput, PasswordInput);

        if (user == null)
        {
            LoginInputError = "Неверные данные";
            PasswordInputError = "Неверные данные";
            return;
        }

        IDayOfWeekService dayOfWeekService = new DayOfWeekService();
        var scheduleVm = new ScheduleScreenViewModel(NavigationService, _authService, _taskService, dayOfWeekService);
        NavigationService.NavigateTo(scheduleVm);
    }

    [RelayCommand]
    private async Task RegisterUser()
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

        var user = new TidyTime.Models.User
        {
            Login = RegisterLoginInput,
            Role = SelectedRole == "Родитель" 
                ? UserRole.Parent 
                : UserRole.Child
        };

        bool success = await _authService.RegisterUserAsync(user, RegisterPasswordInput);

        if (!success)
        {
            RegisterLoginInputError = "Пользователь уже существует";
            return;
        }
        
        IDayOfWeekService dayOfWeekService = new DayOfWeekService();
        var scheduleVm = new ScheduleScreenViewModel(NavigationService, _authService, _taskService, dayOfWeekService);
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
