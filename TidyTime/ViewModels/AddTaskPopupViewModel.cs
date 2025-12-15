using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Models;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class AddTaskPopupViewModel : ObservableObject
{
    private readonly ITaskService _taskService;
    private readonly User _currentUser;
    private readonly DateTime _selectedDate;
    private readonly TaskItem? _existingTask;

    public IRelayCommand CloseCommand { get; }

    public AddTaskPopupViewModel(ITaskService taskService, 
                                Action closeAction, 
                                User currentUser, 
                                DateTime selectedDate, 
                                TaskItem? existingTask = null)
    {
        _taskService = taskService;
        _currentUser = currentUser;
        _selectedDate = selectedDate;
        _existingTask = existingTask;

        CloseCommand = new RelayCommand(closeAction);

        if (currentUser.Role == UserRole.Parent)
        {
            LoadChildrenForParentAsync();
        }
        else if (currentUser.Role == UserRole.Child)
        {
            SelectedChildId = currentUser.Id;
        }

        if (existingTask != null)
        {
            Title = existingTask.Title;
            Description = existingTask.Description;
            StartTime = existingTask.StartTime.TimeOfDay;
            EndTime = existingTask.EndTime.TimeOfDay;
            IsAllDay = existingTask.IsAllDay;
            Difficulty = existingTask.Difficulty;

            IsStartTimeSelected = true;
            IsEndTimeSelected = true;
        }
        else
        {
            StartTime = DateTime.Now.TimeOfDay;
            EndTime = DateTime.Now.AddHours(1).TimeOfDay;
            IsStartTimeSelected = false;
            IsEndTimeSelected = false;
        }
    }

    [ObservableProperty] private string title = "";
    [ObservableProperty] private string description = "";
    [ObservableProperty] private TimeSpan startTime;
    [ObservableProperty] private TimeSpan endTime;
    [ObservableProperty] private bool isAllDay;
    [ObservableProperty] private int difficulty = 1;
    [ObservableProperty] private bool hasTimeConflict;
    [ObservableProperty] private string timeConflictMessage = "";

    [ObservableProperty] 
    private bool _isStartTimeSelected = false;
    [ObservableProperty] 
    private bool _isEndTimeSelected = false;

    [ObservableProperty]
    private ObservableCollection<User> availableChildren = new();

    [ObservableProperty]
    private string? selectedChildId;

    [ObservableProperty]
    private ObservableCollection<int> difficultyLevels = new() { 1, 2, 3, 4, 5 };

    public string StartTimeDisplay => StartTime.ToString(@"hh\:mm");
    public string EndTimeDisplay => EndTime.ToString(@"hh\:mm");

    private async void LoadChildrenForParentAsync()
    {
        try
        {
            var children = await _taskService.GetChildrenForParentAsync(_currentUser.Id);
            AvailableChildren = new ObservableCollection<User>(children);
            
            if (AvailableChildren.Any())
            {
                SelectedChildId = AvailableChildren.First().Id;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading children: {ex.Message}");
        }
    }

    partial void OnStartTimeChanged(TimeSpan value)
    {
        IsStartTimeSelected = true;
        OnPropertyChanged(nameof(StartTimeDisplay));
    }

    partial void OnEndTimeChanged(TimeSpan value)
    {
        IsEndTimeSelected = true;
        OnPropertyChanged(nameof(EndTimeDisplay));
    }

    [RelayCommand]
    private async Task SaveTaskAsync()
    {
        if (string.IsNullOrWhiteSpace(Title)) return;

        var startDateTime = _selectedDate.Date + StartTime;
        var endDateTime = _selectedDate.Date + EndTime;

        if (!IsAllDay)
        {
            await CheckTimeConflictAsync(startDateTime, endDateTime);
            if (HasTimeConflict) return;
        }

        var task = _existingTask ?? new TaskItem();
        
        task.Title = Title;
        task.Description = Description;
        
        if (IsAllDay)
        {
            task.StartTime = _selectedDate.Date;
            task.EndTime = _selectedDate.Date.AddDays(1);
        }
        else
        {
            task.StartTime = startDateTime;
            task.EndTime = endDateTime;
        }
        
        task.IsAllDay = IsAllDay;
        task.Difficulty = Difficulty;
        task.Status = _existingTask?.Status ?? Models.TaskStatus.Pending;
        task.OwnerId = _currentUser.Id;

        if (_currentUser.Role == UserRole.Child)
        {
            task.AssignedChildId = _currentUser.Id;
        }
        else if (_currentUser.Role == UserRole.Parent)
        {
            task.AssignedChildId = SelectedChildId ?? _currentUser.Id;
        }
        else
        {
            task.AssignedChildId = _currentUser.Id;
        }

        if (task.StartTime >= task.EndTime)
        {
            TimeConflictMessage = "Время окончания должно быть позже времени начала";
            HasTimeConflict = true;
            return;
        }

        if (_existingTask == null)
        {
            await _taskService.AddTaskAsync(task);
        }
        else
        {
            await _taskService.UpdateTaskAsync(task);
        }

        CloseCommand.Execute(null);
    }

    private async Task CheckTimeConflictAsync(DateTime start, DateTime end)
    {
        if (IsAllDay)
        {
            HasTimeConflict = false;
            return;
        }

        string targetChildId;
        
        if (_currentUser.Role == UserRole.Child)
        {
            targetChildId = _currentUser.Id;
        }
        else if (_currentUser.Role == UserRole.Parent && SelectedChildId != null)
        {
            targetChildId = SelectedChildId;
        }
        else
        {
            HasTimeConflict = false;
            return;
        }

        var allTasks = await _taskService.GetAllTasksAsync();
        var childTasks = allTasks
            .Where(t => t.AssignedChildId == targetChildId && 
                    t.StartTime.Date == start.Date)
            .ToList();
        
        var conflictingTasks = childTasks
            .Where(t => t.Id != (_existingTask?.Id ?? "") && 
                    !t.IsAllDay &&
                    t.StartTime < end && t.EndTime > start)
            .ToList();

        HasTimeConflict = conflictingTasks.Any();
        
        if (HasTimeConflict)
        {
            var task = conflictingTasks.First();
            TimeConflictMessage = $"Конфликт с задачей: '{task.Title}' ({task.StartTime:HH:mm} - {task.EndTime:HH:mm})";
        }
        else
        {
            TimeConflictMessage = "";
        }
    }

    [RelayCommand]
    private void SelectDifficulty(string level)
    {
        if (int.TryParse(level, out int difficultyLevel) && difficultyLevel >= 1 && difficultyLevel <= 5)
        {
            Difficulty = difficultyLevel;
        }
    }

    [RelayCommand]
    private void ToggleAllDay()
    {
        IsAllDay = !IsAllDay;
        
        if (IsAllDay)
        {
            EndTime = TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(59));
        }
        else
        {
            EndTime = DateTime.Now.AddHours(1).TimeOfDay;
        }
    }
}
