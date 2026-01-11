using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TidyTime.Services;
using TidyTime.Models;
using TidyTime.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using Avalonia.Media;

namespace TidyTime.Games.Memory;

public partial class MemoryGameViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ITaskService _taskService;
    private User? _currentUser;
    
    [ObservableProperty]
    private int _score = 0;
    
    [ObservableProperty]
    private int _combo = 0;
    
    [ObservableProperty]
    private int _level = 1;
    
    [ObservableProperty]
    private string _gameStatus = "Готовьтесь...";
    
    [ObservableProperty]
    private bool _isPlayersTurn = false;
    
    [ObservableProperty]
    private ObservableCollection<CircleViewModel> _circles;
    
    private List<int> _sequence = new();
    private int _currentStep = 0;
    private Random _random = new();
    private bool _isShowingSequence = false;
    
    public MemoryGameViewModel(INavigationService navigationService, 
                              IAuthService authService,
                              ITaskService taskService)
        : base(navigationService)
    {
        _authService = authService;
        _taskService = taskService;
        _currentUser = _authService.GetCurrentUser();
        
        Circles = new ObservableCollection<CircleViewModel>();
        for (int i = 0; i < 12; i++)
        {
            Circles.Add(new CircleViewModel(i, OnCircleClicked));
        }
        
        StartNewGame();
    }
    
    private async void StartNewGame()
    {
        _sequence.Clear();
        _currentStep = 0;
        Score = 0;
        Combo = 0;
        Level = 1;
        GameStatus = "Готовьтесь...";
        
        await Task.Delay(1000);
        await AddNewStepAndShowSequence();
    }
    
    private async Task AddNewStepAndShowSequence()
    {
        IsPlayersTurn = false;
        _isShowingSequence = true;
        GameStatus = "Запоминайте...";
        
        // Добавляем новый шаг в последовательность
        _sequence.Add(_random.Next(0, 12));
        
        // Показываем последовательность
        foreach (var circleId in _sequence)
        {
            await FlashCircle(circleId, Colors.LightBlue, 500);
            await Task.Delay(300);
        }
        
        _currentStep = 0;
        _isShowingSequence = false;
        IsPlayersTurn = true;
        GameStatus = $"Ваш ход! Шагов: {_sequence.Count}";
    }
    
    private async Task FlashCircle(int circleId, Color color, int duration)
    {
        var circle = Circles[circleId];
        circle.FlashColor(color);
        
        await Task.Delay(duration);
        
        circle.ResetColor();
    }
    
    private async void OnCircleClicked(int circleId)
    {
        if (!IsPlayersTurn || _isShowingSequence) return;
        
        // Проверяем правильность нажатия
        if (circleId == _sequence[_currentStep])
        {
            await FlashCircle(circleId, Colors.LightGreen, 300);
            _currentStep++;
            
            if (_currentStep >= _sequence.Count)
            {
                // Вся последовательность угадана
                Combo++;
                Score += 10 * Level;
                
                await FlashAllCircles(Colors.Green, 500);
                
                await Task.Delay(1000);
                
                // Переход на следующий уровень
                Level++;
                GameStatus = $"Отлично! Уровень {Level}";
                
                await Task.Delay(1500);
                await AddNewStepAndShowSequence();
            }
            else
            {
                GameStatus = $"Правильно! Осталось: {_sequence.Count - _currentStep}";
            }
        }
        else
        {
            // Ошибка
            Combo = 0;
            
            await FlashAllCircles(Colors.Red, 800);
            
            GameStatus = "Ошибка! Начинаем заново...";
            
            await Task.Delay(2000);
            StartNewGame();
        }
    }
    
    private async Task FlashAllCircles(Color color, int duration)
    {
        foreach (var circle in Circles)
        {
            circle.FlashColor(color);
        }
        
        await Task.Delay(duration);
        
        foreach (var circle in Circles)
        {
            circle.ResetColor();
        }
    }
    
    [RelayCommand]
    private void ExitGame()
    {
        var vm = new GameMenuViewModel(NavigationService, _authService, _taskService);
        NavigationService.NavigateTo(vm);
    }
}

public class CircleViewModel : ObservableObject
{
    private readonly Action<int> _onClicked;
    private SolidColorBrush _backgroundColor = new SolidColorBrush(Color.Parse("#3A3D5F"));
    
    public int Id { get; }
    
    public SolidColorBrush BackgroundColor
    {
        get => _backgroundColor;
        private set => SetProperty(ref _backgroundColor, value);
    }
    
    public IRelayCommand CircleClickCommand { get; }
    
    public CircleViewModel(int id, Action<int> onClicked)
    {
        Id = id;
        _onClicked = onClicked;
        CircleClickCommand = new RelayCommand(() => _onClicked?.Invoke(Id));
    }
    
    public void FlashColor(Color color)
    {
        BackgroundColor = new SolidColorBrush(color);
    }
    
    public void ResetColor()
    {
        BackgroundColor = new SolidColorBrush(Color.Parse("#3A3D5F"));
    }
}