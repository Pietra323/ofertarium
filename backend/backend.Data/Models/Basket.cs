using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Data.Models.ManyToManyConnections;


namespace backend.Data.Models;

[Table("Basket")]
public class Basket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public virtual User? User { get; set; }
    
    public virtual ICollection<BasketProduct>? BasketProducts { get; set; }
}