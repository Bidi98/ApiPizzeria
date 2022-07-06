namespace ApiPizzeria.Models
{
    public class GetOrderDto
    {
        public int IdOrder { get; set; }
        public DateTime OrderTime { get; set; }
        public List<GetProductOrderDto> Products { get; set; }
    }
}
