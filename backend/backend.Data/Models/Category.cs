using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Data.Models.ManyToManyConnections;


namespace backend.Data.Models;

[Table("Category")]
public class Category
{
    [Key]
    public int Id { get; set; }

    public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
}