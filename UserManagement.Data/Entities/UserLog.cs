using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class UserLog
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }    
    [ForeignKey("UserId")]
    public long UserId { get; set; }
    public string Message { get; set; } = default!;
    public string Changes { get; set; } = default!;
    public DateTime Timestamp { get; set; }
}
