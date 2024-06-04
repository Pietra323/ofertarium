using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace backend.Data.Models;

[Table("Location")]
public class Location
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Country { get; set; }
    
    [Required]
    public string Street { get; set; }
    
    [Required]
    public int HomeNumber { get; set; }
    
    [Required]
    public int ApartmentNumber { get; set; }
    
    [Required]
    public int PostalCode { get; set; }
    [JsonIgnore]
    public int AccountSettingsId { get; set; }
    [JsonIgnore]
    public virtual AccountSettings AccountSettings { get; set; }
}