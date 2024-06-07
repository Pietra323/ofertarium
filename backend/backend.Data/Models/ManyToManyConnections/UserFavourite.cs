using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models.ManyToManyConnections;

[Table("UserFavourite")]
public class UserFavourite
{
    public int UserId { get; set; }
    public virtual User User { get; set; }
    
    public int FavouriteId { get; set; }
    public virtual Favourite Favourite { get; set; }
    
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
}
