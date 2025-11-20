using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using TidyTime.Views;
using TidyTime.Services;

namespace TidyTime;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Отключаем валидацию DataAnnotation, чтобы не было конфликтов с CommunityToolkit
        DisableAvaloniaDataAnnotationValidation();

        if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            // Создаём навигационный сервис для мобильного приложения
            var navigationService = new NavigationService(singleViewPlatform);

            // Задаём стартовый экран
            singleViewPlatform.MainView = new AuthView(navigationService);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Получаем все плагины валидации DataAnnotation
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // Удаляем их, чтобы не было дублирующейся валидации
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
