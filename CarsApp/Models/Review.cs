using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarsApp.Models
{
    public class Review
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Car")]
        public int CarId { get; set; }

        [Display(Name = "Car")]
        [ForeignKey("CarId")]
        public Car Car { get; set; }

        [Display(Name = "Driver")]
        public int DriverId { get; set; }

        [Display(Name = "Driver")]
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }

        [StringLength(50)]
        [Display(Name = "Engine Type")]
        public string? EngineType { get; set; }

        [Display(Name = "Engine Size")]
        public double? EngineSize { get; set; }

        [Display(Name = "Fuel Consumption")]
        public double? FuelConsumption { get; set; }

        [StringLength(255)]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}
