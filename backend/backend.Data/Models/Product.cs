using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models;

[Table("Product")]
public class Product
{
    [Key] 
    public int ProductId { get; set; }
    
    [Required] 
    public string ProductName { get; set; }

    /*
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    */
}