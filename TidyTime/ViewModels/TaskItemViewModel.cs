using System;
using TidyTime.Models;
using TidyTime.Services;

namespace TidyTime.ViewModels;

public class TaskItemViewModel : ViewModelBase
{
    private readonly TaskItem _taskItem;
    private bool _isExpanded;
    
    public TaskItemViewModel(TaskItem taskItem, INavigationService navigationService) 
        : base(navigationService)
    {
        _taskItem = taskItem;
    }
    
    public TaskItem Task => _taskItem;
    
    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetProperty(ref _isExpanded, value);
    }
   
    public string Id => _taskItem.Id;
    public string Title => _taskItem.Title;
    public string Description => _taskItem.Description;
    public DateTime StartTime => _taskItem.StartTime;
    public DateTime EndTime => _taskItem.EndTime;
    public bool IsAllDay => _taskItem.IsAllDay;
    public int Difficulty => _taskItem.Difficulty;
    public TaskStatus Status => _taskItem.Status;
    public string OwnerId => _taskItem.OwnerId;
    public DateTime CreatedAt => _taskItem.CreatedAt;
    public DateTime UpdatedAt => _taskItem.UpdatedAt;
    
    public string DifficultyText => TaskItemHelper.GetDifficultyText(_taskItem.Difficulty);
    public string StatusText => TaskItemHelper.GetStatusText(_taskItem.Status);
    public string TimeRange => TaskItemHelper.GetTimeRange(_taskItem.StartTime, _taskItem.EndTime, _taskItem.IsAllDay);
    public string CardColor => TaskItemHelper.GetCardColor(_taskItem.Difficulty, _taskItem.Status);
    public bool IsTitleStrikethrough => TaskItemHelper.IsTitleStrikethrough(_taskItem.Status);
    public int RewardCoins => TaskItemHelper.GetRewardCoins(_taskItem.Difficulty);
    public bool HasDescription => TaskItemHelper.HasDescription(_taskItem.Description);
}