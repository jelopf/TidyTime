using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;

namespace TidyTime.Games.Snake;

public partial class SnakeGameView : UserControl
{
    private const int CellSize = 25;
    private const int GridSize = 12;
    
    private List<Point> _snake = new();
    private Point _food;
    private Point _direction = new(1, 0);
    private DispatcherTimer _timer = new();
    private DateTime _startTime;
    private int _score = 0;
    private int _highScore = 0;
    
    public SnakeGameView()
    {
        InitializeComponent();
        StartGame();
    }
    
    private void StartGame()
    {
        _snake.Clear();
        _snake.Add(new Point(5, 5));
        _snake.Add(new Point(4, 5));
        _snake.Add(new Point(3, 5));
        
        SpawnFood();
        _startTime = DateTime.Now;
        
        _timer.Interval = TimeSpan.FromMilliseconds(150);
        _timer.Tick += (_, _) => Update();
        _timer.Start();
        
        Render();
    }
    
    private void Update()
    {
        if (_snake.Count == 0) return;
        
        var head = _snake[^1];
        var newHead = new Point(
            (head.X + _direction.X + GridSize) % GridSize,
            (head.Y + _direction.Y + GridSize) % GridSize
        );
        
        // Проверка на столкновение с собой
        if (_snake.Contains(newHead))
        {
            // Сбрасываем змейку
            _snake.Clear();
            _snake.Add(new Point(5, 5));
            _snake.Add(new Point(4, 5));
            _snake.Add(new Point(3, 5));
            
            // Обновляем рекорд
            if (_score > _highScore)
            {
                _highScore = _score;
            }
            _score = 0;
        }
        else
        {
            _snake.Add(newHead);
        }
        
        if (newHead == _food)
        {
            _score += 10;
            SpawnFood();
        }
        else if (_snake.Count > 0)
        {
            _snake.RemoveAt(0);
        }
            
        Render();
    }
    
    private void SpawnFood()
    {
        var rnd = new Random();
        Point p;
        do
        {
            p = new Point(rnd.Next(GridSize), rnd.Next(GridSize));
        }
        while (_snake.Contains(p));
        
        _food = p;
    }
    
    private void Render()
    {
        GameCanvas.Children.Clear();
        
        for (int i = 0; i <= GridSize; i++)
        {
            GameCanvas.Children.Add(new Border
            {
                Width = 1,
                Height = GridSize * CellSize,
                Background = Brushes.Gray,
                Opacity = 0.2,
                Margin = new Thickness(i * CellSize, 0, 0, 0)
            });
            
            GameCanvas.Children.Add(new Border
            {
                Width = GridSize * CellSize,
                Height = 1,
                Background = Brushes.Gray,
                Opacity = 0.2,
                Margin = new Thickness(0, i * CellSize, 0, 0)
            });
        }
        
        for (int i = 0; i < _snake.Count; i++)
        {
            var part = _snake[i];
            var isHead = i == _snake.Count - 1;
            
            GameCanvas.Children.Add(new Border
            {
                Width = CellSize - 4,
                Height = CellSize - 4,
                Background = isHead ? Brushes.DarkGreen : Brushes.Green,
                CornerRadius = new CornerRadius(isHead ? 8 : 4),
                Margin = new Thickness(part.X * CellSize + 2, part.Y * CellSize + 2, 0, 0)
            });
        }
        
        // Рисуем еду
        GameCanvas.Children.Add(new Ellipse
        {
            Width = CellSize - 8,
            Height = CellSize - 8,
            Fill = new SolidColorBrush(Color.Parse("#FF6666")),
            Margin = new Thickness(_food.X * CellSize + 4, _food.Y * CellSize + 4, 0, 0)
        });
        
        // Обновляем статистику
        var elapsed = DateTime.Now - _startTime;
        TimeText.Text = $"{elapsed:mm\\:ss}";
        ScoreText.Text = $"Очки: {_score}";
        HighScoreText.Text = $"Рекорд: {_highScore}";
        LengthText.Text = $"Длина: {_snake.Count}";
    }
    
    private void UpButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_direction.Y != 1)
            _direction = new Point(0, -1);
    }
    
    private void DownButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_direction.Y != -1)
            _direction = new Point(0, 1);
    }
    
    private void LeftButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_direction.X != 1)
            _direction = new Point(-1, 0);
    }
    
    private void RightButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_direction.X != -1)
            _direction = new Point(1, 0);
    }
    
    private void ExitButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _timer.Stop();
        
        if (DataContext is SnakeGameViewModel vm)
        {
            vm.GoBackCommand.Execute(null);
        }
    }
}