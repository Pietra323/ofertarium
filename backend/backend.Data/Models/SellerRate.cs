using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("SellerRate")]
public class SellerRate
{
    [Key]
    public int Id { get; set; } 
}