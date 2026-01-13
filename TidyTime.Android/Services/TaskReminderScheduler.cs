using System;
using Android.App;
using Android.Content;
using TidyTime.Models;

namespace TidyTime.Android.Services;

public class TaskReminderScheduler
{
    private readonly Context _context;

    public TaskReminderScheduler(Context context)
    {
        _context = context;
    }

    public void ScheduleReminder(TaskItem task)
    {
        var reminderTime = CalculateReminderTime(task);

        if (reminderTime <= DateTime.Now)
            return;

        var intent = new Intent(_context, typeof(TaskReminderReceiver));
        intent.PutExtra("title", "Напоминание ⏰");
        intent.PutExtra("message", $"Приготовься {task.Title}");

        var pendingIntent = PendingIntent.GetBroadcast(
            _context,
            task.Id.GetHashCode(),
            intent,
            PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
        );

        var alarmManager = (AlarmManager)_context.GetSystemService(Context.AlarmService)!;

        alarmManager.SetExactAndAllowWhileIdle(
            AlarmType.RtcWakeup,
            new DateTimeOffset(reminderTime).ToUnixTimeMilliseconds(),
            pendingIntent
        );
    }

    public void CancelReminder(TaskItem task)
    {
        var intent = new Intent(_context, typeof(TaskReminderReceiver));

        var pendingIntent = PendingIntent.GetBroadcast(
            _context,
            task.Id.GetHashCode(),
            intent,
            PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
        );

        var alarmManager = (AlarmManager)_context.GetSystemService(Context.AlarmService)!;
        alarmManager.Cancel(pendingIntent);
    }

    private DateTime CalculateReminderTime(TaskItem task)
    {
        if (task.IsAllDay)
        {
            return task.StartTime.Date.AddDays(-1).AddHours(18);
        }

        return task.StartTime.AddHours(-1);
    }
}
