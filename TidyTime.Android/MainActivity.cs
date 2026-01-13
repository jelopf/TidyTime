using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using TidyTime.Android.Services;
using TidyTime.Services;

namespace TidyTime.Android;

[Activity(
    Label = "TidyTime.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    public static TaskReminderScheduler? ReminderScheduler { get; private set; }
    
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        var reminderService = new AndroidReminderService(this);
        MainActivity.ReminderScheduler = new TaskReminderScheduler(this);

        App.CurrentReminderService = reminderService;

        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }
}
