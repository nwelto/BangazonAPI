using System.ComponentModel.DataAnnotations.Schema;

namespace BangazonAPI.Models;

public class OrderProduct
{
    public int Id { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public Product Product { get; set; }

    [ForeignKey("Order")]
    public int OrderId { get; set; }
    public Order Order { get; set; }
}
