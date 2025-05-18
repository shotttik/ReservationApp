using API.Attributes;
using Application.Authentication;
using Application.Common.Results;
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
        [HasPermission(Permission.WorkScheduleManageCompany)]
        public async Task<IActionResult> CreateCompanyWorkSchedules([FromBody] WorkSchedulesCreateRequest request)
        {
            Result result = await workScheduleService.WorkSchedulesCreate(request, isForEmployee: false);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }

        [HttpPut("company")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.WorkScheduleManageCompany)]
        public async Task<IActionResult> UpdateCompanyWorkSchedules([FromBody] WorkSchedulesUpdateRequest request)
        {
            Result result = await workScheduleService.WorkSchedulesUpdate(request, isForEmployee: false);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        [HttpPost("employee")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.WorkScheduleManageUser)]
        public async Task<IActionResult> CreateEmployeeWorkSchedules([FromBody] WorkSchedulesCreateRequest request)
        {
            Result result = await workScheduleService.WorkSchedulesCreate(request, isForEmployee: true);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }

        [HttpPut("employee")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.WorkScheduleManageUser)]
        public async Task<IActionResult> UpdateEmployeeWorkSchedules([FromBody] WorkSchedulesUpdateRequest request)
        {
            Result result = await workScheduleService.WorkSchedulesUpdate(request, isForEmployee: true);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
    }
}
