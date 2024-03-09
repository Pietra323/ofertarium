using System.ComponentModel.DataAnnotations;

namespace backend.Data.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string? ProductName { get; set; }
}