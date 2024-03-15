using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Payment")]
public class Payment
{
    [Key]
    public int Id { get; set; } 
}