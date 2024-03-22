using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("Bucket")]
public class Bucket
{
    [Key]
    public int Id { get; set; }
    
    public virtual User User { get; set; }

    public virtual ICollection<Product> Products { get; set; }
}