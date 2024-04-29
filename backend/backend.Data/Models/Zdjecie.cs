using System.ComponentModel.DataAnnotations;

namespace backend.Data.Models.DataBase;

public class Zdjecie
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Sciezka { get; set; }
        
    public int? ProductId { get; set; }
    public virtual Product? Product { get; set; }
}