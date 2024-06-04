using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace backend.Data.Models;

[Table("Comment")]
public class Comment
{
    [Key]
    public int Id { get; set; } 
    
    [Required]
    public string Content { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public TimeSpan Time { get; set; }
    [JsonIgnore]
    public int UserId;
    [JsonIgnore]
    public virtual User User { get; set; }
}