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
        /// <param name="request">Schedule entries to be applied to the company. The <c>TargetUserId</c> property should not be set for this endpoint.</param>
        /// <returns>Success if creation is successful, or validation errors otherwise.</returns>
        [HttpPost("company")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.WorkScheduleCompanyCreate)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCompanyWorkSchedules([FromBody] WorkSchedulesCreateRequest request)
        {
            var result = await workScheduleService.WorkSchedulesCreate(request, isForEmployee: false);

            return result.ToResponse();
        }
        /// <summary>
        /// Updates the existing company work schedule.
        /// </summary>
        /// <remarks>
        /// All schedule entries must reference existing work schedule IDs.
        /// </remarks>
        /// <param name="request">Updated schedule entries for the company. The <c>TargetUserId</c> property should not be set for this endpoint.</param>
        /// <returns>Success if update is applied.</returns>
        [HttpPut("company")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.WorkScheduleCompanyUpdate)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCompanyWorkSchedules([FromBody] WorkSchedulesUpdateRequest request)
        {
            var result = await workScheduleService.WorkSchedulesUpdate(request, isForEmployee: false);

            return result.ToResponse();
        }
        /// <summary>
        /// Creates the employee's personal work schedule.
        /// </summary>
        /// <remarks>
        /// Must conform to the company's defined schedule boundaries (e.g., hours, days).
        /// SuperAdmins may provide <c>TargetUserId</c> in the request to assign the schedule to another user.
        /// All other roles must omit this property.
        /// </remarks>
        /// <param name="request">Schedule entries for all 7 days of the week. Optionally includes <c>TargetUserId</c> if called by SuperAdmin.</param>
        /// <returns>Success if creation is valid and within company constraints.</returns>
        [HttpPost("employee")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.WorkScheduleUserCreate)]
        public async Task<IActionResult> CreateEmployeeWorkSchedules([FromBody] WorkSchedulesCreateRequest request)
        {
            var result = await workScheduleService.WorkSchedulesCreate(request, isForEmployee: true);

            return result.ToResponse();
        }
        /// <summary>
        /// Updates the employee's personal work schedule.
        /// </summary>
        /// <remarks>
        /// All updated entries must match existing employee schedule IDs.
        /// SuperAdmins may provide <c>TargetUserId</c> to update another user's schedule.
        /// All other roles must omit this property.
        /// </remarks>
        /// <param name="request">Updated work schedule entries. Optionally includes <c>TargetUserId</c> if called by SuperAdmin.</param>
        /// <returns>Success if update passes validation and is within company-defined limits.</returns>
        [HttpPut("employee")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.WorkScheduleUserRead)]
        public async Task<IActionResult> UpdateEmployeeWorkSchedules([FromBody] WorkSchedulesUpdateRequest request)
        {
            var result = await workScheduleService.WorkSchedulesUpdate(request, isForEmployee: true);

            return result.ToResponse();
        }
    }
}
