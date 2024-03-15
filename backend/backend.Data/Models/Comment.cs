using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Comment")]
public class Comment
{
    [Key]
    public int Id { get; set; } 
}