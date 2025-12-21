using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Models;
using TidyTime.Services;
using TidyTime.Views;

namespace TidyTime.ViewModels;

public partial class PraisePopupViewModel : ObservableObject
{
    private const int MaxCustomTextLength = 50;

    private readonly Action _closeAction;

    public string ChildId { get; }
    public string TaskId { get; }

    public ObservableCollection<PraiseOption> PraiseOptions { get; }

    [ObservableProperty]
    private PraiseOption? _selectedPraise;

    [ObservableProperty]
    private string _customPraiseText = string.Empty;

    public PraisePopupViewModel(
        string childId,
        string taskId,
        Action closeAction)
    {
        ChildId = childId;
        TaskId = taskId;
        _closeAction = closeAction;

        PraiseOptions = new ObservableCollection<PraiseOption>
        {
            new("Ты молодец! ⚡"),
            new("Умничка! Горжусь тобой!"),
            new("Ты отлично справился с задачей!"),
            new("Я люблю тебя! ❤")
        };
    }

    partial void OnCustomPraiseTextChanged(string value)
    {
        if (value.Length > MaxCustomTextLength)
            CustomPraiseText = value[..MaxCustomTextLength];

        if (!string.IsNullOrWhiteSpace(value))
        {
            SelectedPraise = null;
        }
    }

    [RelayCommand]
    private void Save()
    {
        var finalText = !string.IsNullOrWhiteSpace(CustomPraiseText)
            ? CustomPraiseText
            : SelectedPraise?.Text;

        if (string.IsNullOrWhiteSpace(finalText))
            return;

        // TODO: Шлпм уведомление

        _closeAction();
    }

    [RelayCommand]
    private void Close()
    {
        _closeAction();
    }
}
