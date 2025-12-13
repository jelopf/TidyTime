using System;

namespace TidyTime.Models;

public static class TaskItemHelper
{
    public static string GetDifficultyText(int difficulty) => difficulty switch
    {
        1 => "Легкая",
        2 => "Средняя",
        3 => "Высокая",
        4 => "Великая",
        5 => "Легендарная",
        _ => "Неизвестно"
    };

    public static string GetStatusText(TaskStatus status) => status switch
    {
        TaskStatus.Pending => "Ожидает",
        TaskStatus.InProgress => "В процессе",
        TaskStatus.Completed => "Выполнено",
        TaskStatus.Cancelled => "Отменено",
        _ => "Неизвестно"
    };

    public static string GetTimeRange(DateTime startTime, DateTime endTime, bool isAllDay)
        => isAllDay ? "Весь день" : $"{startTime:HH:mm}-{endTime:HH:mm}";

    public static string GetCardColor(int difficulty, TaskStatus status)
    {
        if (status == TaskStatus.Completed)
            return "#EEEEEE";
            
        return difficulty switch
        {
            1 => "#B2F6C3", 
            2 => "#F7DAB3", 
            3 => "#EFB3F5", 
            4 => "#FFB3B3",
            5 => "#B3D9FF",
            _ => "#EEEEEE"
        };
    }

    public static bool IsTitleStrikethrough(TaskStatus status)
        => status == TaskStatus.Completed;

    public static int GetRewardCoins(int difficulty) => difficulty switch
    {
        1 => 6,   // Легкая
        2 => 12,  // Средняя
        3 => 24,  // Высокая
        4 => 50,  // Великая
        5 => 150, // Легендарная
        _ => 0
    };

    public static bool HasDescription(string description)
        => !string.IsNullOrWhiteSpace(description);
}