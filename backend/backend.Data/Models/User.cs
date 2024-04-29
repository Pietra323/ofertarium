using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using backend.Data.Models.ManyToManyConnections;

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
        
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }

        public virtual AccountSettings? AccountSettings { get; set; }
        
        public virtual ICollection<Comment>? Comments { get; set; }
        
        public virtual ICollection<Rate>? Rates { get; set; }
        
        public virtual ICollection<UserFavourite>? UserFavourite { get; set; }
        
        public virtual Basket? Basket { get; set; }
        
        public virtual ICollection<AuctionUser>? AuctionUsers { get; set; }
    }
}