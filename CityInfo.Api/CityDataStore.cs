using CityInfo.Api.Models;

namespace CityInfo.Api
{
    public class CityDataStore
    {
        public List<CityDto> Cities { get; set; }
        public CityDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New-York City",
                    Description = "Mafia Capital of The Usa",
                    PointsOfInterests = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=1,
                            Name = "Central Park",
                            Description = "Huge Park in the Center of the city"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name =  "Little Italy",
                            Description = "Historical Italian Neighbourhood"
                        }
                    }
                },
                new CityDto()
                {
                    Id=2,
                    Name = "Los-Angeles",
                    Description = "Worlds Most Entertaining City",
                    PointsOfInterests = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=1,
                            Name = "Hollywood Walk of Fame",
                            Description = "Street with stars on the sidewalk"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name =  "Malibu",
                            Description = "Popular beach on the coast of Pacific Ocean"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Tbilisi",
                    Description= "Tope vaar Braat",
                    PointsOfInterests = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=1,
                            Name = "Funikuliori",
                            Description = "Park on the hill with the views on the city"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name =  "Orbeliani Street",
                            Description = "Old Neighbourhood"
                        }
                    }
                }
            };
        }
    }
}
