using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Data.Models.ManyToManyConnections;

namespace backend.Data.Models;

[Table("Product")]
public class Product
{
    [Key] 
    public int Id { get; set; }
    
    [Required] 
    public string ProductName { get; set; }
    
    public virtual ICollection<OrderProduct>? OrderProducts { get; set; }
    
    public int UserId { get; set; }
    public virtual User User { get; set; }
    
    public virtual ICollection<CategoryProduct>? CategoryProducts { get; set; }
    
    public virtual OnSale? OnSale { get; set; }

    public virtual Auction? Auction { get; set; }

    public int? BucketId { get; set; }
    public virtual Bucket? Bucket { get; set; }
}