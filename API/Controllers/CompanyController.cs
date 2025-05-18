using API.Attributes;
using Application.Authentication;
using Application.Common.Results;
using Application.DTOs.Company;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController :ControllerBase
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;

        }
        [HttpPost("invite")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> InviteMember([FromBody] InviteMemberRequest request)
        {
            var result = await companyService.InviteMember(request.UserAccountID);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
        }
        [HttpGet("invite-accept")]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> InviteAccept([FromQuery] string token)
        {
            var result = await companyService.InviteAccept(token);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        [HttpPost("service")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ServiceCreate)]
        public async Task<IActionResult> CompanyServicesSCreate([FromBody] ServicesCreateRequest request)
        {
            Result result = await companyService.ServicesCreate(request);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        [HttpPut("service")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ServiceUpdate)]
        public async Task<IActionResult> CompanyServicesUpdate([FromBody] ServicesUpdateRequest request)
        {
            Result result = await companyService.ServicesUpdate(request);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        [HttpDelete("service")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ServiceDelete)]
        public async Task<IActionResult> CompanyServiceDelete([FromQuery] int ID)
        {
            Result result = await companyService.ServicesDelete(ID);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        [HttpGet("paged")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.CompanyRead)]
        public async Task<IActionResult> GetPagedCompanies([FromQuery] PagedParameters parameters, CancellationToken cancellationToken)
        {
            var result = await companyService.GetPaged(parameters, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return result.ToProblemDetails();
            }
        }
    }
}
