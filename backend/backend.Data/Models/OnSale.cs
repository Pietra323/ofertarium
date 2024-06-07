using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;


namespace backend.Data.Models;

[Table("OnSale")]
public class OnSale
{
    [Key]
    public int Id { get; set; }

    public decimal OldPrice { get; set; }
    
    public decimal OnSalePrice { get; set; }
    
    public DateTime? ExpirationTime { get; set; }

    public int? ProductId { get; set; }
    public virtual Product Product { get; set; }
    
}