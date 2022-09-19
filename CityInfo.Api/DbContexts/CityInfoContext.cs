using CityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;
        public CityInfoContext(DbContextOptions<CityInfoContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData(
                new City("New York City")
                {
                    Id = 1,
                    Description = "Mafia Capital of the USA"
                },
                new City("Denver")
                {
                    Id = 2,
                    Description = "Smoke weed every day"
                },
                new City("Paris")
                {
                    Id = 3,
                    Description = "Segleblue where is mi mamaa??!!"
                });

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                new PointOfInterest("Little Italy")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "Old Italian Neighbourhood"
                },
                new PointOfInterest("Central Park")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "Fine Park located in the center"
                },
                new PointOfInterest("Aspen")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "Winter Resort in the mountains"
                },
                new PointOfInterest("Eifel Tower")
                {
                    Id = 4,
                    CityId = 3,
                    Description = "Worlds most popular tower"
                },
                new PointOfInterest("Louvre")
                {
                    Id = 5,
                    CityId = 3,
                    Description = "One of the best Museums in the world"
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
