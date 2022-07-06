using System.ComponentModel.DataAnnotations;

namespace ApiPizzeria.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Address { get; set; }
        public string? CurrentRefreshToken { get; set; }
        public DateTime? RefreshTokenExp { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
