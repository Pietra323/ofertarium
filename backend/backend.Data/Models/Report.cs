using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Report")]
public class Report
{
    [Key]
    public int Id { get; set; } 
}