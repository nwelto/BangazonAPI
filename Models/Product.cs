using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BangazonAPI.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; } 

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }


    public int SellerId { get; set; }
    public User Seller { get; set; } 

    public int CategoryId { get; set; }
    public Category Category { get; set; } 
}

