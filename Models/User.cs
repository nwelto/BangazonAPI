using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonAPI.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public string FirebaseUid { get; set; }

    [Required]
    [MaxLength(255)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }

    public bool IsSeller { get; set; }

    // Navigation properties
    public virtual ICollection<Product> Products { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}

