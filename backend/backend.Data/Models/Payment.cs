using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Payment")]
public class Payment
{
    [Key]
    public int Id { get; set; } 
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public int CardNumber { get; set; }

    [Required]
    public string ExpirationDate { get; set; }
    
    [Required]
    public int SecureCode { get; set; }

    public int UserId;
    public User User = new User();
    public AccountSettings AccountSettings { get; set; } = new AccountSettings();
}