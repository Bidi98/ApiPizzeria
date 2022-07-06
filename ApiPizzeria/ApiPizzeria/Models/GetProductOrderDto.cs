namespace ApiPizzeria.Models
{
    public class GetProductOrderDto
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
