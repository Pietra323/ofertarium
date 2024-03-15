using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Auctions")]
public class Auction
{
    [Key]
    public int Id { get; set; } 
}