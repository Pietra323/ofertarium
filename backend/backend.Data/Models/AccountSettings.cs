using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace backend.Data.Models;

[Table("AccountSettings")]
public class AccountSettings
{
    [Key] public int Id { get; set; }
    [JsonIgnore] public virtual ICollection<Location>? Locations { get; set; }
    [JsonIgnore] public virtual User? User { get; set; }
}