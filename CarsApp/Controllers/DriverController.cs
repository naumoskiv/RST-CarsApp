using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CarsApp.Data;
using CarsApp.Models;
using CarsApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarsApp.Controllers
{
    [Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {

        private readonly CarsAppContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly IHostingEnvironment webHostEnvironment;

        public DriverController(CarsAppContext context, IHostingEnvironment hostEnvironment, UserManager<AppUser> userMgr)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            userManager = userMgr;
        }

        // GET: Driver/Reviews/5
        public async Task<IActionResult> Reviews(int? id)
        {
            if (id == null)
            {
                AppUser curruser = await userManager.GetUserAsync(User);
                if (curruser.DriverId != null)
                    return RedirectToAction(nameof(Reviews), new { id = curruser.DriverId });
                else
                    return NotFound();
            }

            Driver driver= await _context.Driver
                .Include(d => d.Reviews).ThenInclude(r => r.Car)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (driver == null)
            {
                return NotFound();
            }

            AppUser user = await userManager.GetUserAsync(User);
            if (driver.Id != user.DriverId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }

            return View(driver);
        }


        // GET: Driver/EditReview/5
        public async Task<IActionResult> EditReview(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Car)
                .Include(r => r.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            AppUser user = await userManager.GetUserAsync(User);
            if (review.DriverId != user.DriverId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }

            var memory = new MemoryStream();

            var reviewEditViewModel = new ReviewEditViewModel
            {
                Review = review
            };

            return View(reviewEditViewModel);
        }


        // POST: Driver/EditReview/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(int id, ReviewEditViewModel entry)
        {
            if (entry.Review.Id != id)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Car)
                .Include(r => r.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            AppUser user = await userManager.GetUserAsync(User);
            if (review.DriverId != user.DriverId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    review.FuelConsumption = entry.Review.FuelConsumption;
                    review.Description = entry.Review.Description;
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(EditReview));
        }

        // GET: Driver/CreateReview
        public async Task<IActionResult> CreateReview(/*int driverId*/)
        {
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Name");
            ViewData["DriverId"] = new SelectList(_context.Driver, "Id", "FullName");
            return View();
        }


        // POST: Driver/EditReview/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReview(/*int driverId, */ReviewCreateViewModel entry)
        {
            if (ModelState.IsValid)
            {
                Review review = new Review
                {
                    Id = entry.Review.Id,
                    DriverId = entry.Review.DriverId,
                    CarId = entry.Review.CarId,
                    EngineSize = entry.Review.EngineSize,
                    EngineType = entry.Review.EngineType,
                    FuelConsumption = entry.Review.FuelConsumption,
                    Description = entry.Review.Description
                };

                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("ReviewDetails", new { Id = review.Id });
            }
            return View();
        }




        // GET: Driver/ReviewDetails/5
        public async Task<IActionResult> ReviewDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Car)
                .Include(r => r.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            AppUser user = await userManager.GetUserAsync(User);
            if (review.DriverId != user.DriverId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }

            return View(review);
        }


        // GET: Driver/Manufacturers
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


        // GET: Driver/ManufacturerDetails/5
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

        // GET: Driver/Cars
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

        // GET: Driver/CarDetails/5
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


        // GET: Driver/ReviewList
        public async Task<IActionResult> ReviewList()
        {
            var carsAppContext = _context.Review.Include(r => r.Car).Include(r => r.Driver);
            return View(await carsAppContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> OthersReview(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Driver)
                .Include(r => r.Car)
                .ThenInclude(c => c.Manufacturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }



        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.Id == id);
        }
    }
}