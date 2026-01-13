using System;
using Android.App;
using Android.Content;
using AndroidX.Core.App;
using Android.OS;

namespace TidyTime.Android.Services;

[BroadcastReceiver(Enabled = true, Exported = false)]
public class TaskReminderReceiver : BroadcastReceiver
{
    private const string ChannelId = "task_reminders";

    public override void OnReceive(Context context, Intent intent)
    {
        CreateChannel(context);

        var title = intent.GetStringExtra("title") ?? "Напоминание";
        var message = intent.GetStringExtra("message") ?? "";

        var notification = new NotificationCompat.Builder(context, ChannelId)
            .SetSmallIcon(Resource.Drawable.Icon)
            .SetContentTitle(title)
            .SetContentText(message)
            .SetAutoCancel(true)
            .Build();

        NotificationManagerCompat.From(context)
            .Notify(new Random().Next(), notification);
    }

    private void CreateChannel(Context context)
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            return;

        var channel = new NotificationChannel(
            ChannelId,
            "Напоминания о задачах",
            NotificationImportance.Default
        );

        var manager = context.GetSystemService(Context.NotificationService) as NotificationManager;
        manager?.CreateNotificationChannel(channel);
    }
}
