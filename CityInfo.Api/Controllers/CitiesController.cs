using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Authorize(Policy = "mustbefromParis")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;
        public CitiesController(ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            string? name, 
            string? searchQuery, 
            int pageNumber = 1, 
            int pageSize = 10
            )
        {

            if(pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            var (cityEntities, paginationMetaData) = await _cityInfoRepository
                .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetaData));

            #region Manual Mapping
            //                        ---- CHEMI VARIANTI ------
            //var res = cityEntities.Select(city => new CityWithoutPointsOfInterestDto()
            //{
            //    Id = city.Id,
            //    Name = city.Name,
            //    Description = city.Description
            //});

            //                   ------ AMIS VARIANTI --------
            //var result = new List<CityWithoutPointsOfInterestDto>();
            //foreach (var city in cityEntities)
            //{
            //    result.Add(new CityWithoutPointsOfInterestDto()
            //    {
            //        Id = city.Id,
            //        Name = city.Name,
            //        Description = city.Description
            //    });
            //}
            #endregion
            return Ok(_mapper.Map<List<CityWithoutPointsOfInterestDto>>(cityEntities));
        }
        /// <summary>
        /// Gets a city by id
        /// </summary>
        /// <param name="id">id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include Points of interest</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns the requested city</response>
        /// <response code="404">Requested City Was not Found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

            if (city == null)
                return NotFound();

            if(includePointsOfInterest)
                return Ok(_mapper.Map<CityDto>(city));

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}
