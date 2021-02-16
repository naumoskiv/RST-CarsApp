using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarsApp.Models;
using Microsoft.AspNetCore.Identity;

namespace CarsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public HomeController(UserManager<AppUser> userMgr)
        {
            userManager = userMgr;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await userManager.GetUserAsync(User);
                if ((await userManager.IsInRoleAsync(appUser, "Admin")))
                {
                    return RedirectToAction("Index", "Cars", null);
                }
                if ((await userManager.IsInRoleAsync(appUser, "Driver")))
                {
                    return RedirectToAction("Reviews", "Driver", null);
                }
            }
            else
            {
                return RedirectToAction("Index", "Guest", null);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
