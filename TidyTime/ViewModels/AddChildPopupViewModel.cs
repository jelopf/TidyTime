using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Models;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class AddChildPopupViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;
    private readonly User _currentUser;
    private readonly Action _closeAction;

    public IRelayCommand CloseCommand { get; }

    public AddChildPopupViewModel(IAuthService authService, 
                                 ITaskService taskService,
                                 Action closeAction, 
                                 User currentUser)
    {
        _authService = authService;
        _taskService = taskService;
        _closeAction = closeAction;
        _currentUser = currentUser;

        CloseCommand = new RelayCommand(closeAction);
    }

    [ObservableProperty] 
    private string _fullName = "";

    [ObservableProperty] 
    private string _childLogin = "";

    [ObservableProperty] 
    private string _fullNameError = "";

    [ObservableProperty] 
    private string _loginError = "";

    [ObservableProperty] 
    private bool _isLoading = false;

    [RelayCommand]
    private async Task AddChildAsync()
    {
        ClearErrors();

        if (string.IsNullOrWhiteSpace(ChildLogin))
        {
            LoginError = "Заполните поле";
            return;
        }
        
        if (string.IsNullOrWhiteSpace(FullName))
        {
            FullNameError = "Заполните поле";
            return;
        }

        try
        {
            IsLoading = true;

            var firebaseService = new FirebaseService();
            var allUsers = await firebaseService.GetAllUsersAsync();

            var childUser = allUsers.FirstOrDefault(u => 
                u.Login.Equals(ChildLogin, StringComparison.OrdinalIgnoreCase));

            if (childUser == null)
            {
                LoginError = "Аккаунт ребёнка не найден";
                return;
            }

            if (childUser.Role != UserRole.Child)
            {
                LoginError = "Этот аккаунт не является детским";
                return;
            }

            var inputName = FullName.Trim();
            var accountName = childUser.FullName.Trim();
            
            inputName = System.Text.RegularExpressions.Regex.Replace(inputName, @"\s+", " ");
            accountName = System.Text.RegularExpressions.Regex.Replace(accountName, @"\s+", " ");

            if (!string.Equals(inputName, accountName, StringComparison.OrdinalIgnoreCase))
            {
                FullNameError = $"Имя Фамилия не совпадает";
                return;
            }

            if (!string.IsNullOrEmpty(childUser.ParentId))
            {
                LoginError = "Этот ребёнок уже привязан к другому родителю.";
                return;
            }

            if (_currentUser.ChildrenIds != null && _currentUser.ChildrenIds.Contains(childUser.Id))
            {
                LoginError = "Этот ребёнок уже привязан к вам";
                return;
            }

            childUser.ParentId = _currentUser.Id;

            if (_currentUser.ChildrenIds == null)
            {
                _currentUser.ChildrenIds = new List<string>();
            }

            _currentUser.ChildrenIds.Add(childUser.Id);

            var saveFirebaseService = new FirebaseService();
            await saveFirebaseService.UpdateUserAsync(childUser);
            await saveFirebaseService.UpdateUserAsync(_currentUser);

            await _authService.UpdateUserAsync(_currentUser);

            _closeAction?.Invoke();
        }
        catch (Exception ex)
        {
            LoginError = $"Ошибка: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ClearErrors()
    {
        FullNameError = "";
        LoginError = "";
    }
}