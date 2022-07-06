using System.ComponentModel.DataAnnotations;

namespace ApiPizzeria.Models
{
    public class OrderProduct
    {
        [Key]
        public int IdOrderProduct { get; set; }
        public int Count { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
