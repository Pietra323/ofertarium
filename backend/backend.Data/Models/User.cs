using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using backend.Data.Models.ManyToManyConnections;
using Newtonsoft.Json;

namespace backend.Data.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; } 
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public bool isAdmin { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order>? Orders { get; set; }
        [JsonIgnore]
        public virtual AccountSettings? AccountSettings { get; set; }
        [JsonIgnore]
        public virtual ICollection<Comment>? Comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserFavourite>? UserFavourite { get; set; }
        [JsonIgnore]
        public virtual Basket? Basket { get; set; }
        [JsonIgnore]
        public virtual ICollection<History>? Histories { get; set; }
        public virtual ICollection<PaymentCard>? PaymentCards { get; set; }
    }
}