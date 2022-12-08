using Purchasing_management.Data;
using Purchasing_management.Data.Entity;
using Purchasing_management.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using LinqKit;
using System.Linq.Expressions;
using System.Linq;
using AutoMapper;

namespace Purchasing_management.Business
{
    public class LoginHandler
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Response> Register(RegisterRequest registerRequest)
        {
            var user = new User
            {
                UserName = registerRequest.UserName,
                Password = registerRequest.Password
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (result.Succeeded) return new Response(System.Net.HttpStatusCode.OK, "Register Successfull");
            else return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Register not successfull.");

        }

        public async Task<Response> Login(LoginResquest loginResquest)
        {
            var user = await _userManager.FindByNameAsync(loginResquest.UserName);

            //if (user == null) 
                //return new ResponseList<User>() 
                //{ 
                    //Code = System.Net.HttpStatusCode.NotFound,
                    //Message = "UserName not found!",
                    //Data = _userManager.Users.ToList()
                //};

            var result = await _signInManager.PasswordSignInAsync(loginResquest.UserName, loginResquest.Password, false, false);

            //if (!result.Succeeded) return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Login fail");

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, loginResquest.UserName)
                //new Claim(ClaimTypes.Name, user.UserName),
                //new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                //new Claim(ClaimTypes.Expired, DateTime.Now.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Utils.GetConfig("Authentication:Jwt:Key")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(Utils.GetConfig("Authentication:Jwt:ExpiryInDays")));

            var token = new JwtSecurityToken(
                Utils.GetConfig("Authentication:Jwt:Issuer"),
                Utils.GetConfig("Authentication:Jwt:Anonymous"),
                claims,
                expires: expiry,
                signingCredentials: creds
            ) ;

            return new ResponseObject<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
