using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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
    
    public int AccountSettingsId { get; set; }
    public virtual AccountSettings AccountSettings { get; set; }
}