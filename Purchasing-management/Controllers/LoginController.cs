using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Purchasing_management.Data.Entity;
using Purchasing_management.Business;
using Purchasing_management.Common;

namespace Purchasing_management.Controllers
{
    [ApiController]
    [Route("api/v{api-version:apiVersion}/logins")]
    public class LoginController : ControllerBase
    {
        private readonly LoginHandler _loginHandler;
        public LoginController(LoginHandler loginHandler)
        {
            _loginHandler = loginHandler;
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromForm] LoginResquest loginResquest)
        {
            var result = await _loginHandler.Login(loginResquest);
            return Helper.TransformData(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Resgister([FromForm] RegisterRequest registerRequest)
        {
            var result = await _loginHandler.Register(registerRequest);
            return Helper.TransformData(result);
        }
    }
}
