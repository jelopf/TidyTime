using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using TidyTime.Views;
using TidyTime.Services;
using TidyTime.ViewModels;

namespace TidyTime;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is ISingleViewApplicationLifetime lifetime)
    {
        var nav = new NavigationService(lifetime);
        var firebaseService = new FirebaseService();
        var taskService = new TaskService();
        var authService = new AuthService(firebaseService);
        
        var currentUser = authService.GetCurrentUser();
        
        if (currentUser != null)
        {
            IDayOfWeekService dayOfWeekService = new DayOfWeekService();
            var scheduleVm = new ScheduleScreenViewModel(nav, authService, taskService, dayOfWeekService);
            lifetime.MainView = new ViewLocator().Build(scheduleVm)!;
            lifetime.MainView.DataContext = scheduleVm;
        }
        else
        {
            var authVm = new AuthViewModel(nav, authService, taskService);
            lifetime.MainView = new ViewLocator().Build(authVm)!;
            lifetime.MainView.DataContext = authVm;
        }
    }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
            BindingPlugins.DataValidators.Remove(plugin);
    }
}
