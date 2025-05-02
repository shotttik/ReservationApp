using API.Attributes;
using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.DTOs.Company;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [Route("api/company")]
    [Controller]
    public class CompanyController :ControllerBase
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;

        }
        [HttpPost("invite")]
        [HasPermission(Permission.EditCompany)]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> InviteMember([FromBody] InviteMemberRequest request)
        {
            var result = await companyService.InviteMember(request.UserAccountID);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
        }
        [HttpGet("accept-invite")]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> AcceptInvite([FromQuery] string token)
        {
            var result = await companyService.AcceptInvite(token);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        [HttpPost("service")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.AddService)]
        public async Task<IActionResult> AddCompanyServices([FromBody] CreateServicesRequest request)
        {
            Result result = await companyService.CreateServices(request);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        [HttpPut("service")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.AddService)]
        public async Task<IActionResult> UpdateCompanyService([FromBody] UpdateServicesRequest request)
        {
            Result result = await companyService.UpdateServices(request);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        [HttpDelete("service")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.AddService)]
        public async Task<IActionResult> DeleteCompanyService([FromQuery] int ID)
        {
            Result result = await companyService.DeleteServices(ID);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
    }
}
