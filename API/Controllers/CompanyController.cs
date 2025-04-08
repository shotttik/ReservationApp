using API.Attributes;
using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.DTOs.Company;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var result = await companyService.InviteMember(request.MemberID);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
        }
        [HttpGet("accept-invite")]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> AcceptInvite([FromQuery] string token)
        {
            var result = await companyService.AcceptInvite(token);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
    }
}
