using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace backend.Data.Models;

[Table("Discount")]
public class Discount
{
    [Key]
    public int Id { get; set; } 
    [JsonIgnore]
    public int AccountSettingsId { get; set; }
    [JsonIgnore]
    public virtual AccountSettings AccountSettings { get; set; }
}