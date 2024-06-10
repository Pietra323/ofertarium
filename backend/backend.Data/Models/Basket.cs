using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Data.Models.ManyToManyConnections;
using backend.Data.Repositories;
using Newtonsoft.Json;


namespace backend.Data.Models;

[Table("Basket")]
public class Basket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [JsonIgnore]
    public virtual User? User { get; set; }
    [JsonIgnore]
    public virtual ICollection<BasketProduct>? BasketProducts { get; set; }
}

public class SummaryBasketDto
{
    public List<BasketProductDto> BasketProducts { get; set; }
    public decimal FinalTotalPrice { get; set; }
}

public class BasketProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
}