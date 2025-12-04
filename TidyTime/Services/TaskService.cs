using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TidyTime.Models;

namespace TidyTime.Services;

public class TaskService : BaseFirebaseService, ITaskService
{
    public async Task<List<TaskItem>> GetTasksForUserAsync(User user)
    {
        var tasks = await FirebaseClient.Child("tasks").OnceAsync<TaskItem>();
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
        var result = await FirebaseClient.Child("tasks").PostAsync(task);

        task.Id = result.Key;

        await FirebaseClient
            .Child("tasks")
            .Child(result.Key)
            .PutAsync(task);
    }

    public async Task UpdateTaskAsync(TaskItem task)
    {
        var allTasks = await FirebaseClient.Child("tasks").OnceAsync<TaskItem>();
        var firebaseTask = allTasks.FirstOrDefault(t => t.Object.Id == task.Id);
        if (firebaseTask != null)
        {
            await FirebaseClient.Child("tasks").Child(firebaseTask.Key).PutAsync(task);
        }
    }

    public async Task DeleteTaskAsync(string taskId)
    {
        var allTasks = await FirebaseClient.Child("tasks").OnceAsync<TaskItem>();
        var firebaseTask = allTasks.FirstOrDefault(t => t.Object.Id == taskId);
        if (firebaseTask != null)
        {
            await FirebaseClient.Child("tasks").Child(firebaseTask.Key).DeleteAsync();
        }
    }
}