using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TidyTime.ViewModels;

public partial class AuthViewModel : ViewModelBase
{
    [ObservableProperty] private int selectedTabIndex = 1; // 0 - вход, 1 - регистрация по умолчанию


    [ObservableProperty] private string selectedRole = "";
    public ObservableCollection<string> Roles { get; } = new() { "Родитель", "Ребёнок" };

    [ObservableProperty] private string login = "";
    [ObservableProperty] private string password = "";
    [ObservableProperty] private string repeatPassword = "";

    [RelayCommand]
    private async Task LoginUser()
    {
        System.Console.WriteLine($"Вход: {Login}");
    }

    [RelayCommand]
    private async Task RegisterUser()
    {
        System.Console.WriteLine($"Регистрация: {Login}, роль: {SelectedRole}");
    }
}

