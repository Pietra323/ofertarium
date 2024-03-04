using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models;

[Table("User")]
public class User
{
    public int Id { get; set; } 
    
    [Required]
    public string? Name { get; set; }
}