using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Data.Models.ManyToManyConnections;


namespace backend.Data.Models;

[Table("Favourite")]
public class Favourite
{
    [Key]
    public int Id { get; set; }

    public virtual ICollection<UserFavourite> UserFavourite { get; set; }
}