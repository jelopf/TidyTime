using TidyTime.Models;

namespace TidyTime.Services;

public interface IReminderService
{
    void ScheduleReminder(TaskItem task);
    void CancelReminder(TaskItem task);
}
