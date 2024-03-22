using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Discount")]
public class Discount
{
    [Key]
    public int Id { get; set; } 
    
    public int AccountSettingsId { get; set; }
    public virtual AccountSettings AccountSettings { get; set; }
}