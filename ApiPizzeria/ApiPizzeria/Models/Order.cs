using System.ComponentModel.DataAnnotations;

namespace ApiPizzeria.Models
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }
        public DateTime OrderTime { get; set; }
        public string Address { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
