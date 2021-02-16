using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarsApp.Models
{
    public class AppUser : IdentityUser
    {
        public string Role { get; set; }

        public int? DriverId { get; set; }

        [Display(Name = "Driver")]
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }
    }
}
