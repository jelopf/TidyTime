using Android.Content;
using TidyTime.Models;
using TidyTime.Services;

namespace TidyTime.Android.Services
{
    public class AndroidReminderService : IReminderService
    {
        private readonly Context _context;

        public AndroidReminderService(Context context)
        {
            _context = context;
        }

        public void ScheduleReminder(TaskItem task)
        {
            var scheduler = new TaskReminderScheduler(_context);
            scheduler.ScheduleReminder(task);
        }

        public void CancelReminder(TaskItem task)
        {
            var scheduler = new TaskReminderScheduler(_context);
            scheduler.CancelReminder(task);
        }
    }
}
