using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;

namespace TidyTime.Services;

public interface INavigationService
{
    void NavigateTo(UserControl view, object? dataContext = null);
}

public class NavigationService : INavigationService
{
    private readonly ISingleViewApplicationLifetime _lifetime;

    public NavigationService(ISingleViewApplicationLifetime lifetime)
    {
        _lifetime = lifetime;
    }

    public void NavigateTo(UserControl view, object? dataContext = null)
    {
        _lifetime.MainView = view;
    }
}

