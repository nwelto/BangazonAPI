using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BangazonAPI.Models;

public class Order
{
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }

    [Required]
    [MaxLength(255)]
    public string Status { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime Created { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; }
}

