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
    
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public AccountSettings AccountSettings { get; set; } = new AccountSettings();
    
    public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    public ICollection<Rate> Rates { get; set; } = new List<Rate>();
}