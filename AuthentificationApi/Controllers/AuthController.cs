using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using AuthentificationApi.Models;
using JWT_Generation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthentificationApi.Controllers
{
    //1
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceProvider _serviceCollection;
        public AuthOptions AuthOptions { get; }

        public AuthController(IOptions<AuthOptions> authOptions)
        {
            AuthOptions = authOptions.Value;
        }

        //4
        private List<Account> _accounts => new List<Account>()
        {
            new()
            {
                Id = Guid.Parse("a85dd8bc-54a9-4948-af05-b9cb2a6a8212"), Email = "1", Password = "1",
                Roles = new Role[] { Role.User }
            },
            new()
            {
                Id = Guid.Parse("99c36c77-026c-48e9-9620-bb484ef47065"), Email = "2", Password = "2",
                Roles = new Role[] { Role.User }
            },
            new()
            {
                Id = Guid.Parse("34499a02-b36a-4e79-a79c-2749499e0423"), Email = "3", Password = "3",
                Roles = new Role[] { Role.User }
            },
            new()
            {
                Id = Guid.Parse("778a77d0-6c9d-4620-966c-b09f968ff6c4"), Email = "4", Password = "4",
                Roles = new[] { Role.Admin }
            },
        };

        //5

        [HttpGet]
        public IActionResult ImAlive()
        {
            var token = "token from GenerateJwt(user)";
            return Ok(new { access_token = token });
        }


        public class TestRequest
        {
            public string Key1 { get; set; }
            public string Key2 { get; set; }
        }

        [Route("test")]
        [HttpPost]
        public IActionResult Test([FromBody] TestRequest testRequest)
        {
            return Ok(testRequest.Key1 + " " + testRequest.Key2);
        }


        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // return Ok(request.Email + " " + request.Password);
            //8
            var user = AuthUser(request.Email, request.Password);
            if (user == null) return Unauthorized();
            var token = GenerateJwt(user);
            //{"access_token":"token from GenerateJwt(user)"}
            return Ok(new { access_token = token });
        }

        private Account AuthUser(string email, string password)
        {
            //7
            return _accounts.SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        string GenerateJwt(Account user)
        {
            var securityKey = AuthOptions.Getsymmetricsecuritykey(); //secret key
            var creditinals = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //подпись
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim("role", role.ToString()));
                //"role" тушта в дотнете есть автомапер который мапит jwt claims авторизационным мехагизмом
                //то есть он автоматом смапится в роли пользователя нормально даже несмотря на то что его нет
                //в стандартном механизме. если другое написать то надо читать шо делать
            }

            var token = new JwtSecurityToken(AuthOptions.Issuer, AuthOptions.Audience, claims,
                expires: DateTime.Now.AddSeconds(AuthOptions.TokenLifeTime), signingCredentials: creditinals);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}