using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using backend.Data.Models.ManyToManyConnections;

namespace backend.Data.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
        

        [JsonIgnore]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}