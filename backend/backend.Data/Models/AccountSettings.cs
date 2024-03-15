using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models;

[Table("AccountSettings")]
public class AccountSettings
{
    [Key] 
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
}