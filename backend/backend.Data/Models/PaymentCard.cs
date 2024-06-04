using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models;

[Table("Payment Cards")]
public class PaymentCard
{
    [Key]
    public int Id { get; set; }
    
    public string OwnerFName { get; set; }
    public string OwnerLName { get; set; }
    public string OwnerNickname { get; set; }
    public long CardNumber { get; set; }
}
