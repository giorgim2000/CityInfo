using AutoMapper;
using CityInfo.Api.Entities;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
    [Authorize]
    [ApiVersion("2.0")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointOfInterestDto> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        public PointsOfInterestController(ILogger<PointOfInterestDto> logger, 
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? 
                throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            //Custom Policy for Authorization

            //var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

            //if(!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
            //{
            //    return Forbid();
            //}

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with the id: {cityId} was not found while accessing point of interest!");
                return NotFound();
            }

            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if(pointOfInterest == null)
                return NotFound();

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
            int cityId, 
            PointOfInterestForCreationDto pointOfInterest)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            
            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            var createdPointOfInterest = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                                    new
                                    {
                                        cityId = cityId,
                                        pointOfInterestId = createdPointOfInterest.Id
                                    }, createdPointOfInterest);
        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> Put(
            int cityId, 
            int pointOfInterestId, 
            PointOfInterestForUpdateDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
                return NotFound();

            var pointOfInterestEntity = await _cityInfoRepository
                                        .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if(pointOfInterestEntity == null)
                return NotFound();

            _mapper.Map(pointOfInterest, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> Patch(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
                return NotFound();

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
                return NotFound();

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(pointOfInterestToPatch))
                return BadRequest(ModelState);

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> Delete(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
                return NotFound();

            var pointOfInterestToDelete = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestToDelete == null)
                return NotFound();

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestToDelete);
            await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send("Point of Intereset Deleted!", $"{pointOfInterestToDelete.Name} has been deleted");

            return NoContent();
        }
    }
}
