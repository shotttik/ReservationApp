using API.Attributes;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO.Branch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/locations")]
    [ApiController]
    [Tags("Branches")]
    public class LocationController :ControllerBase
    {
        private readonly IBranchService branchService;

        public LocationController(IBranchService branchService)
        {
            this.branchService = branchService;
        }

        [HttpGet("countries")]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse<List<CountryDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCountries()
        {
            var result = await branchService.GetCountries();
            return result.ToResponse();
        }

        [HttpGet("countries/{countryId}/states")]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse<List<StateDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStatesByCountry(int countryId)
        {
            var result = await branchService.GetStatesByCountry(countryId);
            return result.ToResponse();
        }
        [HttpGet("states/{stateId}/cities")]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse<List<CityDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCitiesByState(int stateId)
        {
            var result = await branchService.GetCitiesByState(stateId);
            return result.ToResponse();
        }
    }
}
