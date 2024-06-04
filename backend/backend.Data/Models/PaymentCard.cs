using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace backend.Data.Models;

[Table("Payment Cards")]
public class PaymentCard
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string OwnerFName { get; set; }
    [Required]
    public string OwnerLName { get; set; }
    [Required]
    public string OwnerNickname { get; set; }
    [Required]
    public long CardNumber { get; set; }
    [Required]
    public decimal? Balance { get; set; }
    public int? UserId { get; set; }
    public virtual User? User { get; set; }
}
