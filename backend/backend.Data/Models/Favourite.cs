using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Data.Models.ManyToManyConnections;
using Newtonsoft.Json;


namespace backend.Data.Models;

[Table("Favourite")]
public class Favourite
{
    public int Id { get; set; }
    public string Description { get; set; }
    
    public virtual ICollection<UserFavourite> UserFavourites { get; set; }
}
