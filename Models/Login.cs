using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightIdeas.Models
{
    public class Login
    {
        public string Email{ get; set; }

        [Required(ErrorMessage = "Email and password do not match")]
        public string Password { get; set; }
    }
}