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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Title) &&
               StartTime < EndTime &&
               Difficulty >= 1 && Difficulty <= 5 &&
               !string.IsNullOrWhiteSpace(OwnerId);
    }
}