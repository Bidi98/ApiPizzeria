namespace ApiPizzeria.Dto
{
    public class ProductDto
    {
        
        public int IdProduct { get; set; }
        public string image { get; set; }
       
        public string Name { get; set; }
       
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
