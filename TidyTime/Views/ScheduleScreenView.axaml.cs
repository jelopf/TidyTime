using Avalonia.Controls;
using TidyTime.ViewModels;
using TidyTime.Services;

namespace TidyTime.Views;

public partial class ScheduleScreenView : UserControl
{
    public ScheduleScreenView()
    {
        InitializeComponent();
    }

    private void OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is ScheduleScreenViewModel vm && 
            sender is TabControl tabControl &&
            tabControl.SelectedItem is TidyTime.Models.DayOfWeekItem selectedDay)
        {
            vm.SelectDayCommand.Execute(selectedDay);
        }
    }
}
