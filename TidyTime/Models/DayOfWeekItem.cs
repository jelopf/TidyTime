using System;

namespace TidyTime.Models;

public class DayOfWeekItem
{
    public DateTime Date { get; set; }
    public string DayName { get; set; } = "";
    public string DayNumber { get; set; } = "";
    public bool IsSelected { get; set; }
    public bool IsToday { get; set; }
}