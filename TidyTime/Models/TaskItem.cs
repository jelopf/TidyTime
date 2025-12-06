using System;
using System.ComponentModel.DataAnnotations;

namespace TidyTime.Models;

public class TaskItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();       

    [Required]
    [MaxLength(50)]        
    public string Title { get; set; } = "";     

    [MaxLength(255)]        
    public string Description { get; set; } = ""; 

    public DateTime StartTime { get; set; }             
    public DateTime EndTime { get; set; }               
    public bool IsAllDay { get; set; }   
         
    [Range(1, 5)]               
    public int Difficulty { get; set; } = 1;

     public TaskStatus Status { get; set; } = TaskStatus.Pending;

    public string AssignedChildId { get; set; } = "";

    [Required]
    public string OwnerId { get; set; } = "";  

    public string DifficultyText => Difficulty switch
    {
        1 => "Легкая",
        2 => "Средняя",
        3 => "Высокая",
        4 => "Великая",
        5 => "Легендарная",
        _ => "Неизвестно"
    };          

    public string StatusText => Status switch
    {
        TaskStatus.Pending => "Ожидает",
        TaskStatus.InProgress => "В процессе",
        TaskStatus.Completed => "Выполнено",
        TaskStatus.Cancelled => "Отменено",
        _ => "Неизвестно"
    };

    public string TimeRange => IsAllDay 
        ? "Весь день" 
        : $"{StartTime:HH:mm}-{EndTime:HH:mm}";

    public string CardColor => Difficulty switch
    {
        1 => "#B2F6C3", 
        2 => "#F7DAB3", 
        3 => "#EFB3F5", 
        4 => "#FFB3B3",
        5 => "#B3D9FF",
        _ => "#EEEEEE"
    };

    public string CompletedCardColor => "#EEEEEE";

    public bool IsTitleStrikethrough => Status == TaskStatus.Completed;

    public int RewardCoins => Difficulty * 10;

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Title) &&
               StartTime < EndTime &&
               Difficulty >= 1 && Difficulty <= 5 &&
               !string.IsNullOrWhiteSpace(OwnerId);
    }
}
