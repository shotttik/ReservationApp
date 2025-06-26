using API.Attributes;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO.Location;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/locations")]
    [ApiController]
    [Tags("Locations")]
    public class LocationController :ControllerBase
    {
        private readonly ILocationService locationService;

        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [HttpGet("countries")]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse<List<CountryDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCountries()
        {
            var result = await locationService.GetCountries();
            return result.ToResponse();
        }

        [HttpGet("countries/{countryId}/states")]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse<List<StateDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStatesByCountry(int countryId)
        {
            var result = await locationService.GetStatesByCountry(countryId);
            return result.ToResponse();
        }
        [HttpGet("states/{stateId}/cities")]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse<List<CityDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCitiesByState(int stateId)
        {
            var result = await locationService.GetCitiesByState(stateId);
            return result.ToResponse();
        }
    }
}
