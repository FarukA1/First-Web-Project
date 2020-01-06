using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class RegistrationModel
    {
        [Required]
        [EmailAddress]
        [Display (Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage ="The {0} must be at least {2} and at max {1} characters long", MinimumLength = 6)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confrimation password do not match")]
        public string ConfirmPassword { get; set; } 
    }
}
