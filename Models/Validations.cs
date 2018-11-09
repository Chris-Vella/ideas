using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using BrightIdeas.Models;

namespace BrightIdeas.Models
{
    using BrightIdeas.Models;
    public class LoginValidation{
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
  
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RegistrationValidation{
 
        
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z0-9_]+$")]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z0-9_]+$")]
        public string Alias { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [Compare("Password", ErrorMessage = "Your password does not match.")]
        public string PasswordConfirmation { get; set; }
    }
}