﻿using CarsApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarsApp.ViewModels
{
    public class ManufacturerViewModel
    {
        [Required]
        [StringLength(40)]
        [Display(Name = "Manufacturer")]
        public string Name { get; set; }

        [StringLength(40)]
        [Display(Name = "Country")]
        public string? Country { get; set; }

        [StringLength(70)]
        [Display(Name = "Headquarters Location")]
        public string? Headquarters { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Founded")]
        public DateTime? Founded { get; set; }

        [Display(Name = "Manufacturer Picture")]
        public IFormFile? ManufacturerPicture { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}
