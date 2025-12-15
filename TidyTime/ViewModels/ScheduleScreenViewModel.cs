using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
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
    private int _selectedDayIndex;

    [ObservableProperty]
    private ObservableCollection<TaskItemViewModel> _tasks = new();

    [ObservableProperty]
    private ObservableCollection<User> _children = new();

    [ObservableProperty]
    private ObservableCollection<DayOfWeekItem> _weekDays = new();

    [ObservableProperty]
    private User? _selectedChild;

    [ObservableProperty]
    private bool _isParentMode = false;

    [ObservableProperty]
    private bool _showNoChildrenMessage = false;

    [ObservableProperty]
    private bool _showNoTasksMessage = false;

    [ObservableProperty]
    private bool _isLoading = true;

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
    private bool _isAddTaskPopupOpen;

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

        if (_currentUser != null)
        {
            IsParentMode = _currentUser.Role == UserRole.Parent;
        }

        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        try
        {
            if (_currentUser == null) return;

            UpdateDateTitle();
            GenerateWeekDays();
            
            if (IsParentMode)
            {
                await LoadChildrenAsync();
                UpdateNoChildrenMessage();

                if (!Children.Any())
                {
                    Tasks.Clear();
                    return;
                }
            }
            else
            {
                UpdateMessagesVisibility();
            }

            UpdateDisplayNames();
            await LoadTasksForDateAsync();
            CalculateTotalCoins();

            AddTaskPopupViewModel = new AddTaskPopupViewModel(
                _taskService,
                CloseAddTask,
                _currentUser,
                SelectedDate
            );
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void UpdateDisplayNames()
    {
        if (_currentUser == null) return;

        UserDisplayName = _currentUser.FullName; 

        if (IsParentMode && SelectedChild != null)
        {
            ChildDisplayName = SelectedChild.FullName;
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

    partial void OnSelectedDateChanged(DateTime value)
{
    var selectedDay = WeekDays.FirstOrDefault(d => d.Date.Date == value.Date);
    if (selectedDay != null)
    {
        SelectedDayIndex = WeekDays.IndexOf(selectedDay);
    }
    
    UpdateDateTitle();
    LoadTasksForDateAsync().ConfigureAwait(false);
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

        if (IsParentMode && !Children.Any())
        {
            Tasks.Clear();
            UpdateMessagesVisibility();
            return;
        }

        IsLoading = true;
        
        try
        {
            List<TaskItem> taskItems;
            
            if (_currentUser.Role == UserRole.Parent && SelectedChild != null)
            {
                taskItems = await _taskService.GetTasksForChildAsync(_currentUser.Id, SelectedChild.Id);
                taskItems = taskItems.Where(t => t.StartTime.Date == SelectedDate.Date).ToList();
            }
            else
            {
                taskItems = await _taskService.GetTasksForDateAsync(_currentUser, SelectedDate);
            }

            Tasks.Clear();
            foreach (var taskItem in taskItems.OrderBy(t => t.StartTime))
            {
                var taskVm = new TaskItemViewModel(taskItem, NavigationService);
                Tasks.Add(taskVm);
            }
        }
        finally
        {
            IsLoading = false;
            UpdateMessagesVisibility();
        }
    }

    private void UpdateMessagesVisibility()
    {
        ShowNoChildrenMessage = IsParentMode && !Children.Any();
        
        ShowNoTasksMessage = !ShowNoChildrenMessage && !Tasks.Any() && !IsLoading;
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

        UpdateNoChildrenMessage();
    }

    private void UpdateNoChildrenMessage()
    {
        ShowNoChildrenMessage = IsParentMode && !Children.Any();
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

        if (IsParentMode && !Children.Any())
        {
            return;
        }

        AddTaskPopupViewModel = new AddTaskPopupViewModel(
            _taskService,
            CloseAddTask,
            _currentUser,
            SelectedDate  
        );
        
        IsAddTaskPopupOpen = true;
    }

    private void CloseAddTask()
    {
        IsAddTaskPopupOpen = false;
        LoadTasksForDateAsync().ConfigureAwait(false);
        CalculateTotalCoins();
    }

    [RelayCommand]
    private void EditTask(TaskItemViewModel taskVm)
    {
        if (_currentUser == null) return;

        AddTaskPopupViewModel = new AddTaskPopupViewModel(
            _taskService,
            CloseAddTask,
            _currentUser,
            SelectedDate, 
            taskVm.Task 
        );
        
        IsAddTaskPopupOpen = true;
    }

    [RelayCommand]
    private async Task DeleteTaskAsync(TaskItemViewModel taskVm)
    {
        if (_currentUser == null || string.IsNullOrEmpty(taskVm.Id)) return;

        await _taskService.DeleteTaskAsync(taskVm.Id);
        await LoadTasksForDateAsync();
        CalculateTotalCoins();
    }

    [RelayCommand]
    private async Task MarkAsCompletedAsync(TaskItemViewModel taskVm)
    {
        if (_currentUser == null || string.IsNullOrEmpty(taskVm.Id)) return;

        await _taskService.MarkTaskAsCompletedAsync(taskVm.Id);
        await LoadTasksForDateAsync();
        CalculateTotalCoins();
    }

    [RelayCommand]
    private void ToggleTaskExpansion(TaskItemViewModel taskVm)
    {
        if (taskVm == null) return;
        
        taskVm.IsExpanded = !taskVm.IsExpanded;
    }

    [RelayCommand]
    private void SelectChild(User child)
    {
        if (_currentUser?.Role != UserRole.Parent) return;

        SelectedChild = child;
        UpdateDisplayNames();
        UpdateNoChildrenMessage();
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