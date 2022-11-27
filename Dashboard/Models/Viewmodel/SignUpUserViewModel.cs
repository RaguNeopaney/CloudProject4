using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace Dashboard.Models.Viewmodel
{
    public class SignUpUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username require")]
        [Remote(action: "UserNameIsExist", controller: "Account")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email require")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Mobile Number")]
        public long? Mobile { get; set; }

        [Required(ErrorMessage = "Password require")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password require")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match")]
        [Display(Name = "Mobile Number")]
        public string ConfirmPassword { get; set; }
    }
}