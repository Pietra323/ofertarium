using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Tag")]
public class Tag
{
    [Key]
    public int Id { get; set; } 
}