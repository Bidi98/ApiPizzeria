using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiPizzeria.Models;
using ApiPizzeria.Services;

namespace ApiPizzeria.Controllers
{
   
    [ApiController]
   // [Authorize]
    public class ShopController : ControllerBase
    {
        IDatabase data;
        public ShopController(IDatabase database)
        {
            data = database;
        }
        [HttpGet]
        [Route("products")]

        public async Task<IActionResult> getProductsAsync()
        {

            var products = await data.GetProductsAsync(); 

            return Ok(products);

        }
        [Authorize]
        [HttpGet("userAddress")]
        public async Task<IActionResult> GetUserAddressAsync()
        {
            var result = await data.GetUserAddressAsync(User.Claims.Select(u => u.Subject.Name).FirstOrDefault());
            if (result.Item2 != "")
            {
                return StatusCode(404, result.Item2);
            }
            return Ok(result.Item1);
        }
        [Authorize]
        [HttpPost("order")]
        public async Task<IActionResult> SetNewOrderAsync([FromBody] NewOrderDto order)
        {
            var result = await data.SetNewOrderAsync(order, User.Claims.Select(u => u.Subject.Name).FirstOrDefault());
            if (result != "")
            {
                return BadRequest(result);
            }


            return Ok();
        }
        [Authorize]
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var result = await data.GetOrdersAsync(User.Claims.Select(u => u.Subject.Name).FirstOrDefault());
            if (result.Item2 != "")
            {
                return BadRequest(result);
            }


            return Ok(result.Item1);
           
        }

    }
}
