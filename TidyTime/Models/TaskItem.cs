using System;

namespace TidyTime.Models;

public class TaskItem
{
    public string Id { get; set; } = "";               
    public string Title { get; set; } = "";             
    public string Description { get; set; } = "";                
    public DateTime StartTime { get; set; }             
    public DateTime EndTime { get; set; }               
    public bool IsAllDay { get; set; }                  
    public int Difficulty { get; set; }   

    public string OwnerId { get; set; } = "";
    public string ChildId { get; set; } = "";              
}
