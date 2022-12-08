using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing_management.Business
{
    public class LoginResquest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Rememberme { get; set; }
    }

    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmedPassword { get; set; }
    }

    public class LoginResponse
    {
        public bool successfull { get; set; }
        public string error { get; set; }
        public string token { get; set; }
    }
}
