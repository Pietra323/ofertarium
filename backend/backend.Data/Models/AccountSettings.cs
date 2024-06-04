using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models;

[Table("AccountSettings")]
public class AccountSettings
{
    [Key] 
    public int Id { get; set; }
    
    public virtual ICollection<Location>? Locations { get; set; }
    
    public virtual User? User { get; set; }
    
    public virtual ICollection<Discount>? Discounts { get; set; }
}