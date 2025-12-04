using System.Collections.Generic;
using System.Threading.Tasks;
using TidyTime.Models;

namespace TidyTime.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetTasksForUserAsync(User user);
    Task AddTaskAsync(TaskItem task);
    Task UpdateTaskAsync(TaskItem task);
    Task DeleteTaskAsync(string taskId);
}
