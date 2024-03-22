using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models.ManyToManyConnections;

[Table("AuctionUser")]
public class AuctionUser
{
    public int AuctionId { get; set; }
    public virtual Auction Auction { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; }
}