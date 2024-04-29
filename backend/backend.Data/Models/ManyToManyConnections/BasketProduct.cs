using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models.ManyToManyConnections;

[Table("BasketProduct")]
public class BasketProduct
{
    public int ProductId { get; set; }
    public virtual Product product { get; set; }

    public int BasketId { get; set; }
    public virtual Basket Basket { get; set; }

    public int? quantity { get; set; }
}