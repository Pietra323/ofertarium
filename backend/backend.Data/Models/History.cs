using Newtonsoft.Json;

namespace backend.Data.Models;

public class History
{
    public int OrderId { get; set; }
    public int UserId { get; set; } 
    
    public string ProductName { get; set; } 
    [JsonIgnore]
    public virtual User User { get; set; }

    public int ProductId { get; set; }
    [JsonIgnore]
    public virtual Product Product { get; set; }
    public DateTime OrderDate { get; set; }
    public int Quantity { get; set; }
}