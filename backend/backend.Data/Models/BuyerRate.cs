using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("BuyerRates")]
public class BuyerRate
{
    [Key]
    public int Id { get; set; }
    
    public virtual Rate Rate { get; set; }
}