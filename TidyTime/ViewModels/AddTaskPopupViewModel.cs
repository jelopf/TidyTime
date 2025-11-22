using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TidyTime.ViewModels;

public partial class AddTaskPopupViewModel : ObservableObject
{
    public IRelayCommand CloseCommand { get; }

    public AddTaskPopupViewModel(Action closeAction)
    {
        CloseCommand = new RelayCommand(closeAction);
    }
}
