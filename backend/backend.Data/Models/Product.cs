using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Data.Models.DataBase;
using backend.Data.Models.ManyToManyConnections;
using Newtonsoft.Json;

namespace backend.Data.Models;

[Table("Product")]
public class Product
{
    [Key] 
    public int IdProduct { get; set; }
    
    [Required] 
    public string ProductName { get; set; }
    
    [Required] 
    public string Subtitle { get; set; }
    
    [Required] 
    public int amountOf { get; set; }
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public List<int> CategoryIds { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<OrderProduct>? OrderProducts { get; set; }
    
    [JsonIgnore]
    public int? UserId { get; set; }
    
    [JsonIgnore]
    public virtual User? User { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<CategoryProduct>? CategoryProducts { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<BasketProduct>? BasketProducts { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Zdjecie>? Zdjecies { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<History>? Histories { get; set; }
}