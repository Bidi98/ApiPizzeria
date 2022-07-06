using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiPizzeria.Dto;
using ApiPizzeria.Helpers;
using ApiPizzeria.Models;
using ApiPizzeria.Services;

namespace ApiPizzeria.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly IDatabase data;
        private readonly IConfiguration configuration;

        public LoginController(IDatabase data, IConfiguration _configuration)
        {
            this.data = data;
            this.configuration = _configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await data.CheckUserAsync(loginRequest);
            if(user.Item2 != "")
            {
                return StatusCode(404,user.Item2);
            }

            return Ok(new
            {
                accessToken = await CreateJwtTokenAsync(user.Item1),
                refreshToken = await CreateJwtRefreshTokenAsync(user.Item1)
            });
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto user)
        {
            var result = await data.SetNewUserAsync(user);

            if(result.Item2 != "")
            {
                return StatusCode(400, result.Item2);
            }


            return Ok(result.Item1);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(string token)
        {
            Console.WriteLine(token);
            var user = await data.CheckRefreshTokenAsync(token);
            if(user.Item2 != "")
            {
                return StatusCode(401, user.Item2);
            }
            if(user.Item1.RefreshTokenExp < DateTime.Now)
            {
                return StatusCode(401, "Refresh token expired");
            }


            return Ok(new
            {
                accessToken = await CreateJwtTokenAsync(user.Item1),
                refreshToken = await CreateJwtRefreshTokenAsync(user.Item1)
            });

        }
    

       


      

        private async Task<string> CreateJwtTokenAsync(User user)
        {
            Claim[] userclaim = new[]
            {
                new Claim(ClaimTypes.Name,user.Login)
           

            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:7032",
                audience: "https://localhost:7032",
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );



            return new JwtSecurityTokenHandler().WriteToken(token);
         

        }

        private async Task<string> CreateJwtRefreshTokenAsync(User user)
        {
            var refreshToken = await data.SetUserRefreshTokenAsync(user);
            if (refreshToken.Item2 != "")
            {
                //
            }
            return refreshToken.Item1;
        }
    }
}
