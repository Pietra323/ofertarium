using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models;

[Table("Product")]
public class Product
{
    [Key] 
    public int Id { get; set; }
    
    [Required] 
    public string ProductName { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
}