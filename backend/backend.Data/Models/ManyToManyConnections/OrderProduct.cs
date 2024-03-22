using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;


namespace backend.Data.Models.ManyToManyConnections;

[Table("OrderProduct")]
public class OrderProduct
{
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }

    public int ProductId { get; set; }
    public virtual Product Product { get; set; }

    public int Quantity { get; set; }
}