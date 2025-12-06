using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TidyTime.Models;

namespace TidyTime.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetTasksForUserAsync(User user);
    Task<List<TaskItem>> GetTasksForDateAsync(User user, DateTime date);
    Task<List<TaskItem>> GetTasksForChildAsync(string parentId, string childId);
    Task AddTaskAsync(TaskItem task);
    Task UpdateTaskAsync(TaskItem task);
    Task DeleteTaskAsync(string taskId);
    Task MarkTaskAsCompletedAsync(string taskId);
    Task<List<User>> GetChildrenForParentAsync(string parentId);
    Task<List<TaskItem>> GetAllTasksAsync();
    Task<TaskItem?> GetTaskByIdAsync(string taskId);
}
