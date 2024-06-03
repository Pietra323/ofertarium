using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Data.Models.ManyToManyConnections;
using Newtonsoft.Json;


namespace backend.Data.Models;

[Table("Basket")]
public class Basket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [JsonIgnore]
    public virtual User? User { get; set; }
    
    public virtual ICollection<BasketProduct>? BasketProducts { get; set; }
}