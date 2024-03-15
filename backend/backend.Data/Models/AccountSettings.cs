using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models;

[Table("AccountSettings")]
public class AccountSettings
{
    [Key] 
    public int Id { get; set; }
    
    public ICollection<Location> Locations { get; set; } = new List<Location>();
    
    public User User { get; set; } = new User();
}