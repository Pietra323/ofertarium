using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Buckets")]
public class Bucket
{
    [Key]
    public int Id { get; set; } 
}