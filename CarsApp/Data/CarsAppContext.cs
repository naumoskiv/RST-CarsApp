using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarsApp.Data
{
    public class CarsAppContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public CarsAppContext (DbContextOptions<CarsAppContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Car { get; set; }

        public DbSet<Driver> Driver { get; set; }

        public DbSet<Manufacturer> Manufacturer { get; set; }

        public DbSet<Review> Review { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Review>()
                .HasOne<Driver>(p => p.Driver)
                .WithMany(p => p.Reviews)
                .HasForeignKey(p => p.DriverId);

            builder.Entity<Review>()
                .HasOne<Car>(p => p.Car)
                .WithMany(p => p.Reviews)
                .HasForeignKey(p => p.CarId);

            builder.Entity<Car>()
                .HasOne<Manufacturer>(p => p.Manufacturer)
                .WithMany(p => p.Cars)
                .HasForeignKey(p => p.ManufacturerId);
        }
    }
}
