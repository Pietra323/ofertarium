using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Discount")]
public class Discount
{
    [Key]
    public int Id { get; set; } 
}