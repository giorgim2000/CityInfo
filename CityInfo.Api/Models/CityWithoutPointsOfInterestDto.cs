namespace CityInfo.Api.Models
{
    /// <summary>
    /// A Dto for a city without Points of Interest
    /// </summary>
    public class CityWithoutPointsOfInterestDto
    {
        /// <summary>
        /// Id of the city
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// a Name of the city
        /// </summary>
        public string Name { get; set; } = String.Empty;
        /// <summary>
        /// Description of the city
        /// </summary>
        public string? Description { get; set; }
    }
}
