using API.Attributes;
using Application.Common.ResultsErrors;
using Application.DTOs.User;
using Application.Interfaces;
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
        [Authorize]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> InviteMember([FromBody] InviteMemberRequest request)
        {
            var result = await companyService.InviteMember(request.MemberID);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
        }
        [HttpGet("accept-invite")]
        [Authorize]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> AcceptInvite([FromQuery] string token)
        {
            var result = await companyService.AcceptInvite(token);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
    }
}
