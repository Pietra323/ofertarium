using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("OnSale")]
public class OnSale
{
    [Key]
    public int Id { get; set; } 
}