using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BangazonAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsOpen { get; set; }
        public DateTime Created { get; set; }

        public User User { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}




