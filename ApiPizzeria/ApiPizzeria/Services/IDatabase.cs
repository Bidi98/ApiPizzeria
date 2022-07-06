using ApiPizzeria.Dto;
using ApiPizzeria.Models;

namespace ApiPizzeria.Services

{
    public interface IDatabase
    {
        public  Task<List<ProductDto>> GetProductsAsync();
        public Task<Tuple<User,string>> CheckUserAsync(LoginRequest login);
        public Task<Tuple<User,string>> SetNewUserAsync(RegisterUserDto user);
        public Task<Tuple<string,string>> SetUserRefreshTokenAsync(User user);
        public Task<Tuple<User, string>> CheckRefreshTokenAsync(string token);
        public Task<Tuple<string, string>> GetUserAddressAsync(string login);
        public Task<string> SetNewOrderAsync(NewOrderDto order, string login);
        public Task<Tuple<List<GetOrderDto>, string>> GetOrdersAsync(string login);

    }
}
