using CarsApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsApp.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            static async Task CreateUserRoles(IServiceProvider serviceProvider)
            {
                var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

                IdentityResult roleResult;
                //Adding Admin Role
                var roleCheck = await RoleManager.RoleExistsAsync("Admin");
                if (!roleCheck)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
                }
                roleCheck = await RoleManager.RoleExistsAsync("Driver");
                if (!roleCheck)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole("Driver"));
                }

                AppUser user = await UserManager.FindByEmailAsync("admin@carsapp.com");
                if (user == null)
                {
                    var User = new AppUser();
                    User.Email = "admin@carsapp.com";
                    User.UserName = "admin@carsapp.com";
                    User.Role = "Admin";
                    string userPWD = "Admin123";
                    IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                    //Add default User to Role Admin      
                    if (chkUser.Succeeded)
                    {
                        var result1 = await UserManager.AddToRoleAsync(User, "Admin");
                    }
                }
            }


            using (var context = new CarsAppContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<CarsAppContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Car.Any() || context.Manufacturer.Any() || context.Driver.Any())
                {
                    return;   // DB has been seeded
                }

                context.Manufacturer.AddRange(
                    new Manufacturer { /*Id = 1, */Name = "BMW", Country = "Germany",
                        Headquarters = "Munich",
                        Founded = DateTime.Parse("1916-03-07"),
                        ManufacturerPicture = "bmw.png"
                    },
                    new Manufacturer { /*Id = 2, */Name = "Toyota", Country = "Japan",
                        Headquarters = "Aichi",
                        Founded = DateTime.Parse("1937-08-07"),
                        ManufacturerPicture = "toyota.png"
                    },
                    new Manufacturer { /*Id = 3, */Name = "Audi", Country = "Germany",
                        Headquarters = "Inglostadt",
                        Founded = DateTime.Parse("1909-07-16"),
                        ManufacturerPicture = "audi.jpg"
                    },
                    new Manufacturer { /*Id = 4, */Name = "Opel", Country = "Germany",
                        Headquarters = "Russelsheim am Main",
                        Founded = DateTime.Parse("1862-01-21"),
                        ManufacturerPicture = "opel.png"
                    },
                    new Manufacturer { /*Id = 5, */Name = "Ford", Country = "USA",
                        Headquarters = "Detroit",
                        Founded = DateTime.Parse("1903-6-16"),
                        ManufacturerPicture = "ford.png"
                    },
                    new Manufacturer { /*Id = 6, */Name = "Chevrolet", Country = "USA",
                        Headquarters = "Detroit",
                        Founded = DateTime.Parse("1903-11-3"),
                        ManufacturerPicture = "chevrolet.png"
                    },
                    new Manufacturer { /*Id = 7, */Name = "Alfa Romeo", Country = "Italy",
                        Headquarters = "Turin",
                        Founded = DateTime.Parse("1910-6-24"),
                        ManufacturerPicture = "alfaromeo.png"
                    },
                    new Manufacturer { /*Id = 8, */Name = "Mazda", Country = "Japan",
                        Headquarters = "Hiroshima",
                        Founded = DateTime.Parse("1920-01-30"),
                        ManufacturerPicture = "mazda.png"
                    },
                    new Manufacturer { /*Id = 9, */Name = "Honda", Country = "Japan",
                        Headquarters = "Minato",
                        Founded = DateTime.Parse("1948-09-24"),
                        ManufacturerPicture = "honda.png"
                    },
                    new Manufacturer { /*Id = 10, */Name = "Mercedes-Benz", Country = "Germany",
                        Headquarters = "Stuttgart",
                        Founded = DateTime.Parse("1926-06-28"),
                        ManufacturerPicture = "mercedes.png"
                    },
                    new Manufacturer { /*Id = 11, */Name = "Ferrari", Country = "Italy",
                        Headquarters = "Maranello",
                        Founded = DateTime.Parse("1939-09-13"),
                        ManufacturerPicture = "ferrari.png"
                    },
                    new Manufacturer { /*Id = 12, */Name = "Lexus", Country = "Japan",
                        Headquarters = "Aichi",
                        Founded = DateTime.Parse("1989-09-01"),
                        ManufacturerPicture = "lexus.jpg"
                    },
                    new Manufacturer
                    { /*Id = 13, */
                        Name = "Renault",
                        Country = "France",
                        Headquarters = "Boulogne-Billancourt",
                        Founded = DateTime.Parse("1898-10-01"),
                        ManufacturerPicture = "renault.png"
                    }
                );
                context.SaveChanges();

                context.Car.AddRange(
                    new Car
                    {
                        /*Id = 1, */
                        Name = "Celica",
                        ModelYear = 2002,
                        Type = "Coupe",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Toyota").Id,
                        CarPicture = "celica.jpg"
                    },
                    new Car
                    {
                        /*Id = 2, */
                        Name = "3 Series",
                        ModelYear = 2017,
                        Type = "Sedan",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "BMW").Id,
                        CarPicture = "3series.jpg"
                    },
                    new Car
                    {
                        /*Id = 3, */
                        Name = "5 Series",
                        ModelYear = 2020,
                        Type = "Sedan",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "BMW").Id,
                        CarPicture = "5series.jpg"
                    },
                    new Car
                    {
                        /*Id = 4, */
                        Name = "3 Series",
                        ModelYear = 2009,
                        Type = "Coupe",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "BMW").Id,
                        CarPicture = "e92.jpg"
                    },
                    new Car
                    {
                        /*Id = 5, */
                        Name = "Astra",
                        ModelYear = 2010,
                        Type = "Hatchback",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Opel").Id,
                        CarPicture = "astra.jpg"
                    },
                    new Car
                    {
                        /*Id = 6, */
                        Name = "Civic",
                        ModelYear = 1998,
                        Type = "Hatchback",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Honda").Id,
                        CarPicture = "civic.jpg"
                    },
                    new Car
                    {
                        /*Id = 7, */
                        Name = "CX-5",
                        ModelYear = 2019,
                        Type = "SUV",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Mazda").Id,
                        CarPicture = "cx5.jpg"
                    },
                    new Car
                    {
                        /*Id = 8, */
                        Name = "IS",
                        ModelYear = 2009,
                        Type = "Sedan",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Lexus").Id,
                        CarPicture = "is.jpg"
                    },
                    new Car
                    {
                        /*Id = 9, */
                        Name = "Giulia",
                        ModelYear = 2018,
                        Type = "Sedan",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Alfa Romeo").Id,
                        CarPicture = "giulia.jpg"
                    },
                    new Car
                    {
                        /*Id = 10, */
                        Name = "LaFerrari",
                        ModelYear = 2013,
                        Type = "Coupe",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Ferrari").Id,
                        CarPicture = "laferrari.jpg"
                    },
                    new Car
                    {
                        /*Id = 11, */
                        Name = "S Class",
                        ModelYear = 2017,
                        Type = "Sedan",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Mercedes-Benz").Id,
                        CarPicture = "sclass.jpg"
                    },
                    new Car
                    {
                        /*Id = 12, */
                        Name = "Mondeo",
                        ModelYear = 2017,
                        Type = "Estate",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Ford").Id,
                        CarPicture = "mondeo.jpg"
                    },
                    new Car
                    {
                        /*Id = 13, */
                        Name = "Silverado",
                        ModelYear = 2018,
                        Type = "Pick Up",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Chevrolet").Id,
                        CarPicture = "silverado.jpg"
                    },
                    new Car
                    {
                        /*Id = 14, */
                        Name = "Q8",
                        ModelYear = 2020,
                        Type = "SUV",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Audi").Id,
                        CarPicture = "audi_a8.jpg"
                    },
                    new Car
                    {
                        /*Id = 15, */
                        Name = "TT",
                        ModelYear = 2003,
                        Type = "Coupe",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Audi").Id,
                        CarPicture = "tt.jpg"
                    },
                    new Car
                    {
                        /*Id = 16, */
                        Name = "Stelvio",
                        ModelYear = 2019,
                        Type = "SUV",
                        ManufacturerId = context.Manufacturer.Single(m => m.Name == "Alfa Romeo").Id,
                        CarPicture = "stelvio.jpg"
                    }
                );
                context.SaveChanges();

                context.Driver.AddRange(
                    new Driver { /*Id = 1, */FirstName = "Viktor", LastName = "Naumoski", LicenceSince = DateTime.Parse("2016-12-21") },
                    new Driver { /*Id = 2, */FirstName = "Keiichi", LastName = "Tsuchiya", LicenceSince = DateTime.Parse("1980-01-01"),
                    DriverPicture = "keiichi.jpg"},
                    new Driver { /*Id = 3, */FirstName = "Kimi", LastName = "Raikkonen", LicenceSince = DateTime.Parse("1993-01-01"),
                    DriverPicture = "kimi.jpg"},
                    new Driver { /*Id = 4, */FirstName = "Takumi", LastName = "Fujiwara", LicenceSince = DateTime.Parse("2001-05-05"),
                    DriverPicture = "takumi.png"}
                );
                context.SaveChanges();

                context.Review.AddRange(
                    new Review { CarId = 5, DriverId = 1, EngineSize = 2.9, EngineType = "Petrol", FuelConsumption = 6.2,
                        Description = "Truly beautiful car with RWD that set a world record on the Nürburgring race track."},
                    new Review { CarId = 6, DriverId = 2, EngineSize = 2.5, EngineType = "Petrol", FuelConsumption = 11.5,
                        Description = "Luxurious Toyota with sporty characteristics. The most reliable car I've ever owned."}
                );
                context.SaveChanges();
            }
        }

    }
}
