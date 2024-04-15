using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Data.Models.ManyToManyConnections;


namespace backend.Data.Models;

[Table("Auction")]
public class Auction
{
    [Key]
    public int Id { get; set; }

    public int UserId;
    public virtual User User { get; set; }
    
    public virtual Product Product { get; set; }

    public virtual ICollection<AuctionUser> AuctionUsers { get; set; }
}