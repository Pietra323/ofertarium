using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Delivery")]
public class Delivery
{
    [Key]
    public int Id { get; set; } 
    
    [Required]
    public int deliveryName { get; set; }
    
    [Required]
    public double Price { get; set; }

    [Required]
    public string Address { get; set; }
}