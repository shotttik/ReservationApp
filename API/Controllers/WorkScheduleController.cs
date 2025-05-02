using API.Attributes;
using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.DTOs.WorkSchedule;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [Route("api/work-schedule")]
    [ApiController]
    public class WorkScheduleController :ControllerBase
    {
        private readonly IWorkScheduleService workScheduleService;

        public WorkScheduleController(IWorkScheduleService workScheduleService)
        {
            this.workScheduleService = workScheduleService;
        }
        [HttpPost("company")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ManageCompanyWorkSchedule)]
        public async Task<IActionResult> AddCompanyWorkSchedules([FromBody] CreateWorkSchedulesRequest request)
        {
            Result result = await workScheduleService.AddWorkSchedules(request, isForEmployee: false);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }

        [HttpPut("company")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ManageCompanyWorkSchedule)]
        public async Task<IActionResult> UpdateCompanyWorkSchedules([FromBody] UpdateWorkSchedulesRequest request)
        {
            Result result = await workScheduleService.UpdateWorkSchedules(request, isForEmployee: false);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        [HttpPost("employee")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ManageUserWorkSchedule)]
        public async Task<IActionResult> AddEmployeeWorkSchedules([FromBody] CreateWorkSchedulesRequest request)
        {
            Result result = await workScheduleService.AddWorkSchedules(request, isForEmployee: true);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }

        [HttpPut("employee")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ManageUserWorkSchedule)]
        public async Task<IActionResult> UpdateEmployeeWorkSchedules([FromBody] UpdateWorkSchedulesRequest request)
        {
            Result result = await workScheduleService.UpdateWorkSchedules(request, isForEmployee: true);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
    }
}
