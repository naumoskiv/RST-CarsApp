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
    public class DriverViewModel
    {
        [Required]
        [StringLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Licence Since")]
        public DateTime LicenceSince { get; set; }

        [Display(Name = "Driver Picture")]
        public IFormFile? DriverPicture { get; set; }

        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }

        [NotMapped]
        public int? Experience
        {
            get
            {
                TimeSpan span = DateTime.Now - LicenceSince;
                double years = (double)span.TotalDays / 365.2425;
                return (int)years;
            }
        }

        public ICollection<Review> Reviews { get; set; }

    }
}
