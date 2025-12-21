using System;
using System.Collections.Generic;
using BCrypt.Net;

namespace TidyTime.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public UserRole Role { get; set; }
    public string FullName { get; set; } = "";
    public string Login { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public List<string> ChildrenIds { get; set; } = new(); 
    public string ParentId { get; set; } = "";

    public int TotalCoins { get; set; } = 0;
    public int CompletedTasksCount { get; set; } = 0;

    public void SetPassword(string password)
    {
        PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, hashType: HashType.SHA384);
    }
    
    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, PasswordHash, hashType: HashType.SHA384);
    }
}
