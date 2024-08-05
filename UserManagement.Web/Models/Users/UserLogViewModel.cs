using System;
using UserManagement.Services.DTO;

namespace UserManagement.Web.Models.Users;

public class UserLogViewModel
{
    public List<UserLogItemViewModel> Items { get; set; } = new();
}

public class UserLogItemViewModel
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool HasChanges { get; set; }
    public DateTime Timestamp { get; set; }
}

public class UserLogDetailItemViewModel
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public List<UserLogChanges> Changes { get; set; } = new();
    public DateTime Timestamp { get; set; }
    public string ReferrerUrl { get; set; } = string.Empty;
}
