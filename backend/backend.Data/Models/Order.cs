using System.ComponentModel.DataAnnotations;

namespace backend.Data.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
}