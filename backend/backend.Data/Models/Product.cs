using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Data.Models.ManyToManyConnections;

namespace backend.Data.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Subtitle { get; set; }

        [Required]
        public int amountOf { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public List<int> CategoryIds { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public virtual ICollection<UserFavourite>? UserFavourites { get; set; }

        public virtual ICollection<OrderProduct>? OrderProducts { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<CategoryProduct>? CategoryProducts { get; set; }
        public virtual ICollection<BasketProduct>? BasketProducts { get; set; }
        public virtual ICollection<History>? Histories { get; set; }
        public int? OnSaleId { get; set; }
        public virtual OnSale? OnSale { get; set; }
    }
}


public class ProductDTO
{
    public int IdProduct { get; set; }
    public string ProductName { get; set; }
    public string Subtitle { get; set; }
    public int amountOf { get; set; }
    public decimal Price { get; set; }
    public int UserId { get; set; }
    public List<int> CategoryIds { get; set; } = new List<int>();
    public List<string> Photos { get; set; } = new List<string>();
    
}