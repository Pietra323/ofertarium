using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Category")]
public class Category
{
    [Key]
    public int Id { get; set; } 
}