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
    public decimal Price { get; set; }
    
    public List<int> CategoryIds { get; set; }
    
    public virtual ICollection<OrderProduct>? OrderProducts { get; set; }
    
    public int? UserId { get; set; }
    public virtual User? User { get; set; }
    
    public virtual ICollection<CategoryProduct>? CategoryProducts { get; set; }
    
    //public virtual OnSale? OnSale { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<BasketProduct>? BasketProducts { get; set; }
    
    public virtual ICollection<Zdjecie>? Zdjecies { get; set; }
}