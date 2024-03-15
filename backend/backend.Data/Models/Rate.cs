using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;


namespace backend.Data.Models;

[Table("Rate")]
public class Rate
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int OverallRate { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public TimeSpan Time { get; set; }
    
    public BuyerRate BuyerRate { get; set; }
    public SellerRate SellerRate { get; set; }

    public int UserId;
    public User User;
}