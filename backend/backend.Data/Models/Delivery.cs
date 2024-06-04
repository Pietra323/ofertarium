using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace backend.Data.Models;

[Table("Delivery")]
public class Delivery
{
    [Key]
    public int Id { get; set; } 
    
    [Required]
    public string deliveryName { get; set; }
    
    [Required]
    public double Price { get; set; }

    [Required]
    public string Address { get; set; }
    [JsonIgnore]
    public virtual Order Order { get; set; }
}