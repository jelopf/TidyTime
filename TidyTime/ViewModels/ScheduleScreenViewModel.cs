using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Models;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class ScheduleScreenViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;
    private readonly IDayOfWeekService _dayOfWeekService;
    private User? _currentUser;

    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;

    [ObservableProperty]
    private ObservableCollection<TaskItem> _tasks = new();

    [ObservableProperty]
    private ObservableCollection<User> _children = new();

    [ObservableProperty]
    private ObservableCollection<DayOfWeekItem> _weekDays = new();

    [ObservableProperty]
    private User? _selectedChild;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private TaskItem? _selectedTask;

    [ObservableProperty]
    private string _userDisplayName = "";

    [ObservableProperty]
    private string _childDisplayName = "";

    [ObservableProperty]
    private string _dateTitle = "";

    [ObservableProperty]
    private int _totalCoins = 0;

    [ObservableProperty]
    private bool _isPopupOpen;

    public AddTaskPopupViewModel? AddTaskPopupViewModel { get; private set; }

    public ScheduleScreenViewModel(INavigationService navigationService, 
                                  IAuthService authService, 
                                  ITaskService taskService,
                                  IDayOfWeekService dayOfWeekService) 
        : base(navigationService)
    {
        _authService = authService;
        _taskService = taskService;
        _dayOfWeekService = dayOfWeekService;
        _currentUser = authService.GetCurrentUser();

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        if (_currentUser == null) return;

        UpdateDisplayNames();
        UpdateDateTitle();
        GenerateWeekDays();
        
        await LoadChildrenAsync();
        await LoadTasksForDateAsync();
        CalculateTotalCoins();

        AddTaskPopupViewModel = new AddTaskPopupViewModel(
            _taskService,
            CloseAddTask,
            _currentUser.Id,
            SelectedDate
        );
    }

    private void UpdateDisplayNames()
    {
        if (_currentUser == null) return;

        UserDisplayName = _currentUser.Login;

        if (_currentUser.Role == UserRole.Parent && SelectedChild != null)
        {
            ChildDisplayName = SelectedChild.Login;
        }
        else
        {
            ChildDisplayName = UserDisplayName;
        }
    }

    private void UpdateDateTitle()
    {
        var culture = new System.Globalization.CultureInfo("ru-RU");
        DateTitle = $"{SelectedDate.ToString("dddd, d MMMM", culture)}";
        DateTitle = char.ToUpper(DateTitle[0]) + DateTitle.Substring(1);
    }

    private void GenerateWeekDays()
    {
        WeekDays.Clear();
        
        var weekDays = _dayOfWeekService.GenerateWeekDays(SelectedDate);
        
        foreach (var day in weekDays)
        {
            WeekDays.Add(day);
        }
    }

    [RelayCommand]
    private async Task LoadTasksForDateAsync()
    {
        if (_currentUser == null) return;

        IsLoading = true;
        
        try
        {
            List<TaskItem> tasks;
            
            if (_currentUser.Role == UserRole.Parent && SelectedChild != null)
            {
                tasks = await _taskService.GetTasksForChildAsync(_currentUser.Id, SelectedChild.Id);
                tasks = tasks.Where(t => t.StartTime.Date == SelectedDate.Date).ToList();
            }
            else
            {
                tasks = await _taskService.GetTasksForDateAsync(_currentUser, SelectedDate);
            }

            Tasks.Clear();
            foreach (var task in tasks.OrderBy(t => t.StartTime))
            {
                Tasks.Add(task);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task LoadChildrenAsync()
    {
        if (_currentUser == null || _currentUser.Role != UserRole.Parent) return;

        var children = await _taskService.GetChildrenForParentAsync(_currentUser.Id);
        
        Children.Clear();
        foreach (var child in children)
        {
            Children.Add(child);
        }

        if (Children.Any())
        {
            SelectedChild = Children.First();
            UpdateDisplayNames();
        }
    }

    private void CalculateTotalCoins()
    {
        TotalCoins = Tasks
            .Where(t => t.Status == Models.TaskStatus.Completed)
            .Sum(t => t.RewardCoins);
    }

    [RelayCommand]
    private void SelectDay(DayOfWeekItem day)
    {
        foreach (var d in WeekDays)
        {
            d.IsSelected = false;
        }
        
        day.IsSelected = true;
        SelectedDate = day.Date;
        
        UpdateDateTitle();
        LoadTasksForDateAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private void NavigateToToday()
    {
        SelectedDate = DateTime.Today;
        GenerateWeekDays();
        UpdateDateTitle();
        LoadTasksForDateAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private void OpenAddTask()
    {
        if (_currentUser == null) return;

        AddTaskPopupViewModel = new AddTaskPopupViewModel(
            _taskService,
            CloseAddTask,
            _currentUser.Id,
            SelectedDate  
        );
        
        IsPopupOpen = true;
    }

    private void CloseAddTask()
    {
        IsPopupOpen = false;
        LoadTasksForDateAsync().ConfigureAwait(false);
        CalculateTotalCoins();
    }

    [RelayCommand]
    private void EditTaskAsync(TaskItem task)
    {
        if (_currentUser == null) return;

        AddTaskPopupViewModel = new AddTaskPopupViewModel(
            _taskService,
            CloseAddTask,
            _currentUser.Id,
            SelectedDate, 
            task
        );
        
        IsPopupOpen = true;
    }

    [RelayCommand]
    private async Task DeleteTaskAsync(TaskItem task)
    {
        if (_currentUser == null || string.IsNullOrEmpty(task.Id)) return;

        await _taskService.DeleteTaskAsync(task.Id);
        await LoadTasksForDateAsync();
        CalculateTotalCoins();
    }

    [RelayCommand]
    private async Task MarkAsCompletedAsync(TaskItem task)
    {
        if (_currentUser == null || string.IsNullOrEmpty(task.Id)) return;

        await _taskService.MarkTaskAsCompletedAsync(task.Id);
        await LoadTasksForDateAsync();
        CalculateTotalCoins();
    }

    [RelayCommand]
    private void SelectChild(User child)
    {
        if (_currentUser?.Role != UserRole.Parent) return;

        SelectedChild = child;
        UpdateDisplayNames();
        LoadTasksForDateAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private void GoToProfile(string role)
    {
        if (_currentUser == null) return;

        if (_currentUser.Role == UserRole.Child)
        {
            var vm = new ChildProfileViewModel(NavigationService, _authService, _taskService);
            NavigationService.NavigateTo(vm);
        }
        else
        {
            var vm = new ParentProfileViewModel(NavigationService, _authService, _taskService);
            NavigationService.NavigateTo(vm);
        }
    }

    [RelayCommand]
    private void GoToMenu()
    {
        var vm = new MenuViewModel(NavigationService, _authService, _taskService);
        NavigationService.NavigateTo(vm);
    }

    [RelayCommand]
    private void NavigateToPreviousWeek()
    {
        SelectedDate = SelectedDate.AddDays(-7);
        GenerateWeekDays();
        UpdateDateTitle();
        LoadTasksForDateAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private void NavigateToNextWeek()
    {
        SelectedDate = SelectedDate.AddDays(7);
        GenerateWeekDays();
        UpdateDateTitle();
        LoadTasksForDateAsync().ConfigureAwait(false);
    }
}