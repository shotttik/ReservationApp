using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.WorkSchedule;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/schedules")]
    [ApiController]
    [Tags("Company & Employee Work Schedules")]
    public class WorkScheduleController :ControllerBase
    {
        private readonly IWorkScheduleService workScheduleService;

        public WorkScheduleController(IWorkScheduleService workScheduleService)
        {
            this.workScheduleService = workScheduleService;
        }
        /// <summary>
        /// Creates a full work schedule for the company.
        /// </summary>
        /// <remarks>
        /// The schedule must include all 7 days of the week. Can only be created if it doesn't already exist.
        /// </remarks>
        /// <param name="request">List of work schedule entries for the company.</param>
        /// <returns>Success if creation is successful, or validation errors otherwise.</returns>
        [HttpPost("company")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.WorkScheduleManageCompany)]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCompanyWorkSchedules([FromBody] WorkSchedulesCreateRequest request)
        {
            Result result = await workScheduleService.WorkSchedulesCreate(request, isForEmployee: false);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        /// <summary>
        /// Updates the existing company work schedule.
        /// </summary>
        /// <remarks>
        /// All schedule entries must reference existing work schedule IDs.
        /// </remarks>
        /// <param name="request">Updated schedule entries for the company.</param>
        /// <returns>Success if update is applied.</returns>
        [HttpPut("company")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.WorkScheduleManageCompany)]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCompanyWorkSchedules([FromBody] WorkSchedulesUpdateRequest request)
        {
            Result result = await workScheduleService.WorkSchedulesUpdate(request, isForEmployee: false);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        /// <summary>
        /// Creates the employee's personal work schedule.
        /// </summary>
        /// <remarks>
        /// Must conform to the company's defined schedule boundaries (e.g., hours, days).
        /// </remarks>
        /// <param name="request">Schedule entries for all 7 days of the week.</param>
        /// <returns>Success if creation is valid and within company constraints.</returns>
        [HttpPost("employee")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.WorkScheduleManageUser)]
        public async Task<IActionResult> CreateEmployeeWorkSchedules([FromBody] WorkSchedulesCreateRequest request)
        {
            Result result = await workScheduleService.WorkSchedulesCreate(request, isForEmployee: true);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
        /// <summary>
        /// Updates the employee's personal work schedule.
        /// </summary>
        /// <remarks>
        /// All updated entries must match existing employee schedule IDs.
        /// </remarks>
        /// <param name="request">Updated work schedule entries.</param>
        /// <returns>Success if update passes validation and is within company-defined limits.</returns>
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
