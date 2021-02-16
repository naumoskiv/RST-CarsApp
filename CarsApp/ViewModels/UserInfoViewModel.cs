using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarsApp.ViewModels
{
    public class UserInfoViewModel
    {
        [Display(Name = "User")]
        public string UserDetails { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "User ID")]
        public string Id { get; set; }

        [Display(Name = "Hashed Password")]
        public string PasswordHash { get; set; }

        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password (Again)")]
        [Compare("NewPassword", ErrorMessage = "Entered Password does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
