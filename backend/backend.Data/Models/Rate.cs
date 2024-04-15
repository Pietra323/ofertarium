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
    
    public virtual BuyerRate BuyerRate { get; set; }
    public virtual SellerRate SellerRate { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; }
}