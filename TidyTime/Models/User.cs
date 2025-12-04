using System.Collections.Generic;

namespace TidyTime.Models;

public class User
{
    public string Id { get; set; } = "";
    public UserRole Role { get; set; }
    public string Login { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public List<string> ChildrenIds { get; set; } = new(); 
    public string ParentId { get; set; } = "";
}
