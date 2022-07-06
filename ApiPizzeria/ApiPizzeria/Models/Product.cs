using System.ComponentModel.DataAnnotations;

namespace ApiPizzeria.Models
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }
        public string PathImage { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
