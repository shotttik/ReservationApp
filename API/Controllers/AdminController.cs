using API.Attributes;
using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("API/[controller]/[action]")]
    [ApiController]
    public class AdminController :ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }
        [HttpPost]
        [HasPermission(Permission.AddUser)]
        [Logging(LoggingType.ExceptBody)]
        public async Task<IActionResult> Add([FromBody] AddRequest request)
        {
            Result result = await adminService.AddUser(request);

            return result.IsSuccess ? Ok() : result.ToProblemDetails();
        }

        [HttpPut]
        [HasPermission(Permission.UpdateUser)]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> Update([FromBody] UpdateRequest request)
        {
            Result result = await adminService.UpdateUser(request);

            return result.IsSuccess ? Ok() : result.ToProblemDetails();
        }
    }
}
