using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Favourite")]
public class Favourite
{
    [Key]
    public int Id { get; set; } 
}