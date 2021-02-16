using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarsApp.Controllers
{
    public class GuestController : Controller
    {

        private readonly CarsAppContext _context;
        private readonly IHostingEnvironment webHostingEnvironment;

        public GuestController(CarsAppContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            webHostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        // GET: Guest/Manufacturers
        public async Task<IActionResult> Manufacturers(string sortOrder, string searchString)
        {
            ViewData["CountrySortParm"] = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            var manufacturers = from m in _context.Manufacturer select m;
            manufacturers = manufacturers.Include(m => m.Cars);

            if (!String.IsNullOrEmpty(searchString))
            {
                manufacturers = manufacturers.Where(m => m.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "type_desc":
                    manufacturers = manufacturers.OrderBy(m => m.Country);
                    break;
                default:
                    manufacturers = manufacturers.OrderBy(m => m.Name);
                    break;
            }

            return View(await manufacturers.AsNoTracking().ToListAsync());
        }


        // GET: Guest/ManufacturerDetails/5
        public async Task<IActionResult> ManufacturerDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturer
                .Include(m => m.Cars)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturer);
        }

        // GET: Guest/Cars
        public async Task<IActionResult> Cars(string sortOrder, string searchString)
        {
            ViewData["TypeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            var carsAppContext = from c in _context.Car select c;

            carsAppContext = _context.Car.Include(c => c.Manufacturer).Include(c => c.Reviews).
                ThenInclude(r => r.Driver);

            if (!String.IsNullOrEmpty(searchString))
            {
                carsAppContext = carsAppContext.Where(c => c.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "type_desc":
                    carsAppContext = carsAppContext.OrderBy(c => c.Type);
                    break;
                default:
                    carsAppContext = carsAppContext.OrderBy(c => c.Manufacturer.Name);
                    break;
            }

            return View(await carsAppContext.AsNoTracking().ToListAsync());
        }

        // GET: Guest/CarDetails/5
        public async Task<IActionResult> CarDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .Include(c => c.Manufacturer)
                .Include(c => c.Reviews).ThenInclude(r => r.Driver)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
    }
}