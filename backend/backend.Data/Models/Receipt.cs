using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace backend.Data.Models;

[Table("Receipt")]
public class Receipt
{
    [Key]
    public int Id { get; set; } 
    [JsonIgnore]
    public virtual Order? Order { get; set; }
}