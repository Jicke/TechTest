using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users;

public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserListItemViewModel
{
    public long Id { get; set; }

    [DisplayName("Forename")]
    [Required(ErrorMessage = "Forename is required")]
    public string Forename { get; set; } = string.Empty;

    [DisplayName("Surname")]
    [Required(ErrorMessage = "Surname is required")]
    public string Surname { get; set; } = string.Empty;

    [DisplayName("Email")]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = string.Empty;

    [DisplayName("Account Active")]
    [Required(ErrorMessage = "Active is required")]
    public bool IsActive { get; set; }

    [DisplayName("Date of Birth")]
    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    public UserLogViewModel Logs { get; set; } = new();
}
