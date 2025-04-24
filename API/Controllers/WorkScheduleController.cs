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
        [HttpPost]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ManageCompanyWorkSchedule)]
        public async Task<IActionResult> AddCompanyWorkSchedules([FromBody] AddWorkSchedulesRequest request)
        {
            Result result = await workScheduleService.AddCompanyWorkSchedules(request);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }

        [HttpPut]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ManageCompanyWorkSchedule)]
        public async Task<IActionResult> UpdateCompanyWorkSchedules([FromBody] UpdateWorkSchedulesRequest request)
        {
            Result result = await workScheduleService.UpdateCompanyWorkSchedules(request);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
    }
}
