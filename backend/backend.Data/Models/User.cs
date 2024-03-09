using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Models;

[Table("User")]
public class User
{
    [Key]
    public int Id { get; set; } 
    
    [Required]
    public string? Name { get; set; }
    
    [Required]
    public string? LastName { get; set; }
    
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? email { get; set; }
}