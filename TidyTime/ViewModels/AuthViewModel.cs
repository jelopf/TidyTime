using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TidyTime.ViewModels;

public partial class AuthViewModel : ViewModelBase
{
    [ObservableProperty] private string selectedRole = "";
    public ObservableCollection<string> Roles { get; } = new() { "Родитель", "Ребёнок" };
    [ObservableProperty] private string login = "";
    [ObservableProperty] private string password = "";
    [ObservableProperty] private string repeatPassword = "";

    [RelayCommand]
    private async Task Register()
    {
        // заглушка
        await Task.Delay(500);
        System.Console.WriteLine($"Регистрация: {Login}, роль: {SelectedRole}");
    }
}
