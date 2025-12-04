using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TidyTime.Models;

namespace TidyTime.Services;

public class TaskService : ITaskService
{
    private readonly FirebaseClient _firebaseClient;

    public TaskService()
    {
        _firebaseClient = new FirebaseClient("https://tidytime-d27eb-default-rtdb.firebaseio.com/");
    }

    public async Task<List<TaskItem>> GetTasksForUserAsync(User user)
    {
        var tasks = await _firebaseClient.Child("tasks").OnceAsync<TaskItem>();
        var all = tasks.Select(t => t.Object).ToList();

        if (user.Role == UserRole.Child)
        {
            return all
                .Where(t => t.OwnerId == user.Id)
                .ToList();
        }

        if (user.Role == UserRole.Parent)
        {
            return all
                .Where(t => 
                    t.OwnerId == user.Id || 
                    user.ChildrenIds.Contains(t.OwnerId))
                .ToList();
        }

        return new List<TaskItem>();
    }

    public async Task AddTaskAsync(TaskItem task)
    {
        var result = await _firebaseClient.Child("tasks").PostAsync(task);

        task.Id = result.Key;

        await _firebaseClient
            .Child("tasks")
            .Child(result.Key)
            .PutAsync(task);
    }

    public async Task UpdateTaskAsync(TaskItem task)
    {
        var allTasks = await _firebaseClient.Child("tasks").OnceAsync<TaskItem>();
        var firebaseTask = allTasks.FirstOrDefault(t => t.Object.Id == task.Id);
        if (firebaseTask != null)
        {
            await _firebaseClient.Child("tasks").Child(firebaseTask.Key).PutAsync(task);
        }
    }

    public async Task DeleteTaskAsync(string taskId)
    {
        var allTasks = await _firebaseClient.Child("tasks").OnceAsync<TaskItem>();
        var firebaseTask = allTasks.FirstOrDefault(t => t.Object.Id == taskId);
        if (firebaseTask != null)
        {
            await _firebaseClient.Child("tasks").Child(firebaseTask.Key).DeleteAsync();
        }
    }
}