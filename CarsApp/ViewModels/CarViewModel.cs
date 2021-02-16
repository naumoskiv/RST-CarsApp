using CarsApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarsApp.ViewModels
{
    public class CarViewModel
    {
        [Required]
        [StringLength(40)]
        [Display(Name = "Model")]
        public string Name { get; set; }

        [Range(1900, 2030)]
        [Display(Name = "Model Year")]
        public int? ModelYear { get; set; }

        [StringLength(40)]
        [Display(Name = "Chassis Type")]
        public string? Type { get; set; }

        [Display(Name = "Car Picture")]
        public IFormFile? CarPicture { get; set; }

        [Display(Name = "Manufacturer")]
        public int ManufacturerId { get; set; }

        [Display(Name = "Manufacturer")]
        [ForeignKey("ManufacturerId")]
        public Manufacturer Manufacturer { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
