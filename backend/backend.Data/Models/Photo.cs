using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models;

public class Photo
{
    [Key]
    public int Id { get; set; }
    
    public string Url { get; set; }
    
    public string Description { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
}

public class PhotoDTO
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }

}