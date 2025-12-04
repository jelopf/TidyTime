using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Models;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public partial class AddTaskPopupViewModel : ObservableObject
{
    private readonly ITaskService _taskService;
    private readonly string _userId;

    public IRelayCommand CloseCommand { get; }

    public AddTaskPopupViewModel(ITaskService taskService, Action closeAction, string userId)
    {
        _taskService = taskService;
        _userId = userId;

        CloseCommand = new RelayCommand(closeAction);
    }

    [ObservableProperty] private string title = "";
    [ObservableProperty] private string description = "";
    [ObservableProperty] private DateTime startTime = DateTime.Now;
    [ObservableProperty] private DateTime endTime = DateTime.Now.AddHours(1);
    [ObservableProperty] private bool isAllDay;
    [ObservableProperty] private int difficulty;

    [RelayCommand]
    private async Task SaveTaskAsync()
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid().ToString(),
            Title = Title,
            Description = Description,
            StartTime = StartTime,
            EndTime = EndTime,
            IsAllDay = IsAllDay,
            Difficulty = Difficulty,
            OwnerId = _userId
        };

        await _taskService.AddTaskAsync(task);

        CloseCommand.Execute(null);
    }
}
