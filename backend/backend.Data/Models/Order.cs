using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using backend.Data.Models.ManyToManyConnections;


namespace backend.Data.Models;

[Table("Order")]
public class Order
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    
    public int UserId { get; set; }
    public virtual User User { get; set; }
    
    public virtual Receipt Receipt { get; set; }
    
    public virtual Delivery Delivery { get; set; }

}