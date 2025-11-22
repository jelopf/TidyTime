using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using TidyTime.ViewModels;

namespace TidyTime.Services;

public interface INavigationService
{
    void NavigateTo(ViewModelBase viewModel);
}

public class NavigationService : INavigationService
{
    private readonly ISingleViewApplicationLifetime _lifetime;

    public NavigationService(ISingleViewApplicationLifetime lifetime)
    {
        _lifetime = lifetime;
    }

    public void NavigateTo(ViewModelBase viewModel)
    {
        var view = (UserControl)new ViewLocator().Build(viewModel)!;
        view.DataContext = viewModel;
        _lifetime.MainView = view;
    }
}
