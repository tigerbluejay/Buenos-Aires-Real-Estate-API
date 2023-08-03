using BuenosAiresRealEstate.API.Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApartmentComplex> ApartmentComplexes { get; set; }
        public DbSet<ApartmentUnit> ApartmentUnits { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // here we seed data into the database
            modelBuilder.Entity<ApartmentComplex>().HasData(
                new ApartmentComplex
                {
                    Id = 1,
                    ComplexName = "Juncal Suites",
                    Address = "Juncal 2736",
                    Amenities = "Pool, Playroom, Laundry Room",
                    Owner = "JM Iriarte",
                    ImageUrl = Path.Combine("Images", "Juncal.jpg"),
                    CreatedDate = DateTime.Now
                },
                new ApartmentComplex
                {
                    Id = 2,
                    ComplexName = "Guise Suites",
                    Address = "Guise 1784",
                    Amenities = "Playroom, Laundry Room",
                    Owner = "JM Iriarte",
                    ImageUrl = Path.Combine("Images", "Guise.jpg"),
                    CreatedDate = DateTime.Now
                },
                new ApartmentComplex
                {
                    Id = 3,
                    ComplexName = "Beruti Tower",
                    Address = "Beruti 3891",
                    Amenities = "Pool, Playroom",
                    Owner = "MH Jaunarena",
                    ImageUrl = Path.Combine("Images", "Beruti.jpg"),
                    CreatedDate = DateTime.Now
                },
                new ApartmentComplex
                {
                    Id = 4,
                    ComplexName = "Paraguay Complex",
                    Address = "Paraguay 1800",
                    Amenities = "Pool, Laundry Room",
                    Owner = "MH Iriarte",
                    ImageUrl = Path.Combine("Images", "Paraguay.jpg"),
                    CreatedDate = DateTime.Now
                },
                new ApartmentComplex
                {
                    Id = 5,
                    ComplexName = "Alcorta Palace",
                    Address = "Alcorta 1900",
                    Amenities = "Pool",
                    Owner = "OA Iriarte",
                    ImageUrl = Path.Combine("Images", "Alcorta.jpg"),
                    CreatedDate = DateTime.Now
                },
                new ApartmentComplex
                {
                    Id = 6,
                    ComplexName = "Santa Fe Apartments",
                    Address = "Santa Fe 2000",
                    Amenities = "Playroom",
                    Owner = "HM Iriarte",
                    ImageUrl = Path.Combine("Images", "SantaFe.jpg"),
                    CreatedDate = DateTime.Now
                },
                new ApartmentComplex
                {
                    Id = 7,
                    ComplexName = "Mansilla Suites",
                    Address = "Mansilla 1000",
                    Amenities = "",
                    Owner = "OA Iriarte",
                    ImageUrl = Path.Combine("Images", "Mansilla.jpg"),
                    CreatedDate = DateTime.Now
                });

        }
    }
}
