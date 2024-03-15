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
    
    [Required]
    public ICollection<OrderProduct> OrderProducts { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public Receipt Receipt { get; set; }
}