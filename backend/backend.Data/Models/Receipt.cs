using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Receipt")]
public class Receipt
{
    [Key]
    public int Id { get; set; } 
    
    public Order Order { get; set; }
}