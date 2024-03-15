using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("WishList")]
public class WishList
{
    [Key]
    public int Id { get; set; } 
}