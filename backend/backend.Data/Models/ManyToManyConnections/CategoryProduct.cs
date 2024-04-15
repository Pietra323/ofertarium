using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models.ManyToManyConnections;

[Table("CategoryProduct")]
public class CategoryProduct
{
    public int? ProductId { get; set; }
    public virtual Product? Product { get; set; }

    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
}