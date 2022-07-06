using Microsoft.EntityFrameworkCore;
using ApiPizzeria.Dto;
using ApiPizzeria.Helpers;
using ApiPizzeria.Models;

namespace ApiPizzeria.Services
{
    
    public class ShopDatabase : IDatabase
    {
        ShopContext context;
        string errorMessage;

        public ShopDatabase()
        {
            errorMessage = "";
            context = new ShopContext();
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var result = await context.Products
                .Select(x => new ProductDto
                {
                    IdProduct = x.IdProduct,
                    Name = x.Name,
                    Price = x.Price,
                    Description = x.Description,
                    image = Convert.ToBase64String(System.IO.File.ReadAllBytes(Path.Combine(Environment
                    .CurrentDirectory,x.PathImage)))
                })
                .ToListAsync();
            return result;
        }

        public async Task<Tuple<User,string>> CheckUserAsync(LoginRequest login)
        {
            errorMessage = "";
            var user = await context.Users
                .Where(e => e.Login == login.Login && e.Password == login.Password)
                .FirstOrDefaultAsync();
            if(user == null)
            {
                errorMessage = "Wrong email or password";
                return new Tuple<User, string>(null, errorMessage);
            }

            return new Tuple<User, string>(user, errorMessage);

        }

        public async Task<Tuple<User,string>> SetNewUserAsync(RegisterUserDto user)
        {
            Console.WriteLine($"login:{user.Login} password:{user.Password}");

            errorMessage = "";
      

            var checkLogin = await context.Users
                .Where(context => context.Login == user.Login)
                .FirstOrDefaultAsync();
            if (checkLogin != null)
            {
                errorMessage = "Email already used";
                return new Tuple<User, string>(null, errorMessage);
            }
            var userId = 1;
            if(context.Users.ToArray().Length > 0)
            {
                userId = await context.Users.MaxAsync(e => e.IdUser) + 1;
            }
            User u = new User()
            {
                IdUser = userId,
                Login = user.Login,
                Password = user.Password

            };
            await context.Users.AddAsync(u);
            var result = await context.SaveChangesAsync();
            return new Tuple<User, string>(u, errorMessage);
        }

        public async Task<Tuple<string, string>> SetUserRefreshTokenAsync(User user)
        {
            errorMessage = "";
            string refreshToken = SecurityHelpers.GenerateRefreshToken();
            user.CurrentRefreshToken = refreshToken;
            user.RefreshTokenExp = DateTime.Now.AddDays(1);
            var result  = await context.SaveChangesAsync();

            if (result < 1)
            {
                errorMessage = "Database error";
                return new Tuple<string, string>(null, errorMessage);
            }
            return new Tuple<string, string>(refreshToken, errorMessage);
        }

        public async Task<Tuple<User, string>> CheckRefreshTokenAsync(string token)
        {
            errorMessage = "";
            var user = await context.Users.Where(u => u.CurrentRefreshToken == token).FirstOrDefaultAsync();

            if(user == null)
            {
                errorMessage = "Invalide refresh token2";
                return new Tuple<User, string>(null, errorMessage);
            }

            return new Tuple<User,string>(user, errorMessage);

        }

        public async Task<Tuple<string, string>> GetUserAddressAsync(string login)
        {
            errorMessage = "";
            Console.WriteLine(login);
            var result = await context.Users
                .Where(u => u.Login == login)
                .Select(u => u.Address)
                .FirstOrDefaultAsync();
            
            if(result == null)
            {
                errorMessage = "Invalide login";
                return new Tuple<string, string>(null, errorMessage);
            }
            return new Tuple<string,string>(result,errorMessage);
        }

        public async Task<string> SetNewOrderAsync(NewOrderDto order,string login)
        {
            errorMessage = "";
            var userId = await context.Users
                .Where(u => u.Login == login)
                .Select(u => u.IdUser)
                .FirstOrDefaultAsync();

            if(userId == 0)
            {
                errorMessage = "Invalide user login";
                return errorMessage;
            }

            var orderId = 1;
            if(context.Orders.ToArray().Length > 0)
            {
                orderId = context.Orders.Max(u => u.IdOrder) + 1;
            }

            Order newOrder = new Order
            {
                IdOrder = orderId,
                OrderTime = DateTime.Now,
                Address = order.Address,
                UserId = userId

            };
            context.Orders.Add(newOrder);
            var orderProductId = 0;
            if(context.OrderProducts.ToArray().Length > 0)
            {
                orderProductId = context.OrderProducts
                    .Max(p => p.IdOrderProduct) + 1;
            }
            for( int i = 0; i < order.ProductCount.Length; i += 2)
            {
                orderProductId++;
                context.OrderProducts.Add(new OrderProduct
                {
                    IdOrderProduct = orderProductId,
                    Count = order.ProductCount[i + 1],
                    OrderId = newOrder.IdOrder,
                    ProductId = order.ProductCount[i]

                }) ;
            }
            var result = await context.SaveChangesAsync();

            if(result < 1)
            {
                errorMessage = "database Error";
            }



            return errorMessage;
        }

        public async Task<Tuple<List<GetOrderDto>, string>> GetOrdersAsync(string login)
        {
            errorMessage = "";
            var userId = await context.Users
                .Where(u => u.Login == login)
                .Select(u => u.IdUser)
                .FirstOrDefaultAsync();
            if (userId == 0)
            {
                errorMessage = "Invalide user login";
                return new Tuple<List<GetOrderDto>, string>(null, errorMessage);
            }
            var result = context.Orders
                .Where(u => u.UserId == userId)
                .Select(u => new GetOrderDto
                {
                    IdOrder = u.IdOrder,
                    OrderTime = u.OrderTime,
                    Products = context.OrderProducts
                    .Where(o => o.OrderId == u.IdOrder)
                    .Select(o => new GetProductOrderDto
                    {
                        IdProduct = o.ProductId,
                        Count = o.Count,
                        Name = o.Product.Name,
                        Price = o.Product.Price
                    }).ToList()
                }).ToList();

            return new Tuple<List<GetOrderDto>, string>(result, errorMessage);
        }
    }
}
