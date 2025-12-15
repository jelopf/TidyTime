using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TidyTime.Models;

namespace TidyTime.Services;

public class TaskService : BaseFirebaseService, ITaskService
{
    public async Task<List<TaskItem>> GetTasksForUserAsync(User user)
    {
        var tasks = await GetAllTasksAsync();
        
        if (user.Role == UserRole.Child)
        {
            return tasks
                .Where(t => t.AssignedChildId == user.Id)
                .ToList();
        }

        if (user.Role == UserRole.Parent)
        {
            return tasks
                .Where(t => 
                    t.OwnerId == user.Id || 
                    user.ChildrenIds.Contains(t.AssignedChildId))
                .ToList();
        }

        return new List<TaskItem>();
    }

    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        var tasks = await FirebaseClient.Child("tasks").OnceAsync<TaskItem>();
        return tasks.Select(t => 
        {
            var task = t.Object;
            task.Id = t.Key;
            return task;
        }).ToList();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(string taskId)
    {
        var allTasks = await GetAllTasksAsync();
        return allTasks.FirstOrDefault(t => t.Id == taskId);
    }

    public async Task AddTaskAsync(TaskItem task)
    {
        if (string.IsNullOrEmpty(task.AssignedChildId) && 
            !string.IsNullOrEmpty(task.OwnerId))
        {
            task.AssignedChildId = task.OwnerId;
        }
        
        var result = await FirebaseClient.Child("tasks").PostAsync(task);
        task.Id = result.Key;
        await FirebaseClient.Child("tasks").Child(result.Key).PutAsync(task);
    }
    
    public async Task UpdateTaskAsync(TaskItem task)
    {
        if (string.IsNullOrEmpty(task.Id))
            return;
            
        await FirebaseClient.Child("tasks").Child(task.Id).PutAsync(task);
    }

    public async Task DeleteTaskAsync(string taskId)
    {
        await FirebaseClient.Child("tasks").Child(taskId).DeleteAsync();
    }

    public async Task<List<TaskItem>> GetTasksForDateAsync(User user, DateTime date)
    {
        var tasks = await GetTasksForUserAsync(user);
        
        return tasks
            .Where(t => t.StartTime.Date == date.Date)
            .OrderBy(t => t.StartTime)
            .ToList();
    }

    public async Task<List<TaskItem>> GetTasksForChildAsync(string parentId, string childId)
    {
        var allTasks = await GetAllTasksAsync();
        
        return allTasks
            .Where(t => t.AssignedChildId == childId)
            .OrderBy(t => t.StartTime)
            .ToList();
    }

    public async Task MarkTaskAsCompletedAsync(string taskId)
    {
        var task = await GetTaskByIdAsync(taskId);
        
        if (task != null)
        {
            task.Status = Models.TaskStatus.Completed;
            await UpdateTaskAsync(task);
        }
    }

    public async Task<List<User>> GetChildrenForParentAsync(string parentId)
    {
        var allUsers = await FirebaseClient.Child("users").OnceAsync<User>();
        
        return allUsers
            .Select(u => u.Object)
            .Where(u => u.ParentId == parentId || 
                       (u.Role == UserRole.Child && u.Id == parentId))
            .ToList();
    }
}