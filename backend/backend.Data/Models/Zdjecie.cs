using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace backend.Data.Models.DataBase;

public class Zdjecie
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Sciezka { get; set; }
    [JsonIgnore]
    public int? ProductId { get; set; }
    [JsonIgnore]
    public virtual Product? Product { get; set; }
}