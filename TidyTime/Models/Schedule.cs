using System;
using System.Collections.Generic;

namespace TidyTime.Models;

public class Schedule
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string OwnerId { get; set; } = "";
    public DateTime Date { get; set; }
    public List<string> TaskIds { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}