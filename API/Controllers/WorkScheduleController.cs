using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.WorkSchedule;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO.WorkSchedule;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/schedules")]
    [ApiController]
    [Tags("Work Schedules")]
    public class WorkScheduleController :ControllerBase
    {
        private readonly IWorkScheduleService workScheduleService;

        public WorkScheduleController(IWorkScheduleService workScheduleService)
        {
            this.workScheduleService = workScheduleService;
        }
        /// <summary>
        /// Creates a new daily work schedule entry for a specific user and day.
        /// </summary>
        /// <remarks>
        /// Requires valid working hours, no overlaps on the same day, and permission to access the user.
        /// Required role: <strong>Accessible by SuperAdmin, CompanyAdmin, CompanyMember</strong>
        /// </remarks>
        /// <param name="request">The work schedule details for a specific day and user.</param>
        /// <returns>Success if the entry is valid and saved.</returns>
        [HttpPost]
        [HasPermission(Permission.WorkScheduleUserCreate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> CreateWorkSchedule([FromBody] WorkScheduleCreateRequest request)
        {
            var result = await workScheduleService.Create(request);
            return result.ToResponse();
        }
        /// <summary>
        /// Updates an existing daily work schedule entry.
        /// </summary>
        /// <remarks>
        /// Validates overlaps and ensures the requester has permission to edit the user’s schedule.
        /// Required role: <strong>Accessible by SuperAdmin, CompanyAdmin, CompanyMember</strong>
        /// </remarks>
        /// <param name="request">The updated schedule info for the specific entry ID.</param>
        /// <returns>Success if update is valid and completed.</returns>
        [HttpPut]
        [HasPermission(Permission.WorkScheduleUserUpdate)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateWorkSchedule([FromBody] WorkScheduleUpdateRequest request)
        {
            var result = await workScheduleService.Update(request);
            return result.ToResponse();
        }
        /// <summary>
        /// Deletes an existing work schedule entry by ID.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible by SuperAdmin, CompanyAdmin, CompanyMember</strong>
        /// </remarks>
        /// <param name="id">The ID of the schedule entry to delete.</param>
        /// <returns>Success if deletion is allowed and completed.</returns>
        [HttpDelete("{id:int}")]
        [HasPermission(Permission.WorkScheduleUserDelete)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteWorkSchedule([FromRoute] int id)
        {
            var result = await workScheduleService.Delete(id);
            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves all daily work schedule entries for a specific user.
        /// </summary>
        /// <remarks>
        /// Returns a list of schedule entries grouped by days of the week.
        /// Required role: <strong>Accessible by SuperAdmin, CompanyAdmin, or the user themselves</strong>.
        /// Access is restricted based on role and user-company relationships.
        /// </remarks>
        /// <param name="userId">The ID of the user whose work schedules are being requested.</param>
        /// <returns>A list of work schedule entries if access is authorized.</returns>
        [HttpGet("users/{userId:int}")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<List<WorkScheduleDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserSchedules([FromRoute] int userId)
        {
            var result = await workScheduleService.GetAllForUser(userId);

            return result.ToResponse();
        }

    }
}
