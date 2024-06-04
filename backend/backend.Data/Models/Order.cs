using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using backend.Data.Models.ManyToManyConnections;
using Newtonsoft.Json;


namespace backend.Data.Models;

[Table("Order")]
public class Order
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    [JsonIgnore]
    public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; }
    [JsonIgnore]
    public virtual Receipt Receipt { get; set; }
    [JsonIgnore]
    public virtual Delivery Delivery { get; set; }

}