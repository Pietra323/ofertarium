using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Data.Models.DataBase;
using backend.Data.Models.ManyToManyConnections;
using Newtonsoft.Json;


namespace backend.Data.Models;

[Table("Category")]
public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Nazwa { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<CategoryProduct>? CategoryProducts { get; set; }
}

public class CategoryDTO
{ 
    public int Id { get; set; }
    public string Nazwa { get; set; }
    public string Description { get; set; }
}