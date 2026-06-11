using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Admin;
using Application.Common.Requests.Company;
using Application.Common.Requests.SubscriptionPlan;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO;
using Domain.DTO.Company;
using Domain.DTO.User;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/companies")]
    [ApiController]
    [Tags("Companies")]
    public class CompanyController :ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IBranchService _branchService;
        private readonly ICompanySubscriptionService _companySubscriptionService;

        public CompanyController(
            ICompanyService companyService,
            IBranchService branchService,
            ICompanySubscriptionService companySubscriptionService)
        {
            _companyService = companyService;
            _branchService = branchService;
            _companySubscriptionService = companySubscriptionService;
        }
        /// <summary>
        /// Retrieves a paginated list of companies.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible by everyone</strong><br/><br/>
        /// <b>Paging and filtering parameters:</b><br/>
        /// <b>Sortable / Filterable Fields:</b>
        /// <ul>
        /// <li><c>ID</c></li>
        /// <li><c>Name</c></li>
        /// <li><c>Description</c></li>
        /// <li><c>IN</c></li>
        /// <li><c>Email</c></li>
        /// <li><c>Phone</c></li>
        /// <li><c>Type</c></li>
        /// <li><c>ActiveStatus</c></li>
        /// <li><c>CreatedAt</c></li>
        /// <li><c>UpdatedAt</c></li>
        /// </ul>
        /// Filtering Example: name~=Company 4||email~=Company40,id==3107
        /// </remarks>
        /// <param name="parameters">Pagination parameters including page number, size, and search filters.</param>
        /// <param name="cancellationToken">Request cancellation token.</param>
        /// <returns>Paged list of company records.</returns>
        [HttpGet()]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<CompanyDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrievePaged([FromQuery] PagedParameters parameters, CancellationToken cancellationToken)
        {
            var result = await _companyService.RetrievePaged(parameters, cancellationToken);

            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves detailed information about a specific company.
        /// </summary>
        /// <param name="id">The ID of the company to retrieve.</param>
        /// <returns>Returns the company details if found.</returns>
        /// <remarks>Required role: <strong>Accessible by everyone</strong></remarks>
        [HttpGet("{id:int}")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<CompanyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _companyService.Get(id);

            return result.ToResponse();
        }
        /// <summary>
        /// Sends a company employeeship invitation to a user.
        /// </summary>
        /// <remarks>
        /// Only company admins can invite users to join their company. 
        /// The user must currently be a public user.
        /// Required role: <strong>CompanyAdmin</strong><br/><br/>
        /// Required permission: <strong>InvitationSend</strong>
        /// </remarks>
        /// <param name="request">Contains the user account ID to invite.</param>
        /// <returns>A secure token (for dev/testing) or email notification result.</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("invitations")]
        [HasPermission(Permission.InvitationSend)]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InviteEmployee([FromBody] InviteEmployeeRequest request)
        {
            var result = await _companyService.InviteEmployee(request);

            return result.ToResponse();
        }
        /// <summary>
        /// Accepts a company invitation using a secure token.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible by everyone</strong>
        /// </remarks>
        /// <param name="token">The invitation token received by email.</param>
        /// <returns>Success result if invitation is valid and accepted.</returns>
        [HttpGet("invitations/accept")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InviteAccept([FromQuery] string token)
        {
            var result = await _companyService.InviteAccept(token);

            return result.ToResponse();
        }
        /// <summary>
        /// Uploads one or more media files for the specified company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows uploading company multiple media files.
        ///
        /// <para><b>Required Roles:</b> SuperAdmin, CompanyAdmin</para>
        /// <br/><br/>
        /// <para><b>Required Permission:</b> CompanyUpdate</para>
        /// <para><b>Max File Size:</b> 1 MB (1,048,576 bytes)</para>
        /// <para><b>Allowed File Types:</b> image/jpeg, image/png</para>
        /// </remarks>
        /// <param name="id">The ID of the company to retrieve.</param>
        /// <param name="request">The request containing the media files to upload.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A result returns address of the image or failure of the upload operation.</returns>
        [Tags("Company Media")]
        [HttpPost("{id:int}/media")]
        [HasPermission(Permission.CompanyUpdate)]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<List<string>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadMedia([FromRoute] int id, [FromForm] UploadCompanyMediaRequest request, CancellationToken cancellationToken)
        {
            var result = await _companyService.UploadMedia(id, request, cancellationToken);

            return result.ToResponse();
        }

        /// <summary>
        /// Updates the description of the authenticated user's company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a CompanyAdmin to update the <c>Description</c> <c>Phone</c>field of their own company.  
        /// <strong>IMPORTANT</strong>: Also allows update branches, so branches should be provided, provided list of branches will changes existing.
        /// The company is determined from the route param context.
        /// Required role: <strong>SuperAdmin,CompanyAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanyUpdatePartial</strong>
        /// </remarks>
        /// <param name="id">The ID of the target company for which the employee is being created.</param>
        /// <param name="request">The request containing the new description.</param>
        /// <returns>No content on success; appropriate error response on failure.</returns>
        [HttpPatch("{id:int}")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanyUpdatePartial)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PartialUpdate([FromRoute] int id, [FromBody] CompanyPartialUpdateRequest request)
        {
            var result = await _companyService.Update(id, request);
            return result.ToResponse();
        }
        /// <summary>
        /// Creates a new company employee user account for the specified company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a CompanyAdmin or SuperAdmin to create a new user account (CompanyEmployee role)
        /// for a specific company identified by <paramref name="id"/>.  
        /// 
        /// The request must contain all required information including personal details and login credentials.  
        /// Email addresses must be unique in the system.  
        /// A verification token is automatically generated and assigned to the new user.
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanyEmployeeCreate</strong>
        /// </remarks>
        /// <param name="id">The ID of the target company for which the employee is being created.</param>
        /// <param name="request">The request containing new employee details and login credentials.</param>
        /// <returns>
        /// Success response if the user is created; appropriate error response if email already exists
        /// or access is denied.
        /// </returns>
        [Tags("Company Employees")]
        [HttpPost("{id:int}/employees")]
        [HasPermission(Permission.CompanyEmployeeCreate)]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateEmployee(
            [FromRoute] int id,
            [FromBody] EmployeeCreateRequest request)
        {
            var result = await _companyService.CreateEmployee(id, request);
            return result.ToResponse();
        }

        /// <summary>
        /// Updates the profile details of a company employee.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a CompanyAdmin to update an existing employee’s first name, last name, gender, and date of birth.
        /// Only authenticated users with appropriate access to the specified company can perform this action.
        /// The employee is identified by their UserLoginData ID.
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanyEmployeeUpdate</strong>
        /// </remarks>
        /// <param name="id">The ID of the company in the route.</param>
        /// <param name="request">The update request containing new profile data.</param>
        /// <returns>No content on success; appropriate error response on failure.</returns>
        [Tags("Company Employees")]
        [HttpPatch("{id:int}/employees")]
        [HasPermission(Permission.CompanyEmployeeUpdate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEmployee(
            [FromRoute] int id,
            [FromBody] EmployeeUpdateRequest request)
        {
            var result = await _companyService.UpdateEmployee(id, request);
            return result.ToResponse();
        }

        /// <summary>
        /// Deletes a company employee from the specified company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a CompanyAdmin to soft delete or permanently delete a company employee (with role CompanyEmployee).  
        /// Only authenticated users with access to the specified company can perform this action.  
        /// The deletion can be a soft delete (default) or a force delete (permanent).
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanyEmployeeDelete</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company in the route.</param>
        /// <param name="employeeID">The ID of the employee to delete (UserLoginData ID).</param>
        /// <param name="force">Whether to permanently delete the employee (true) or perform a soft delete (false).</param>
        /// <returns>No content on success; appropriate error response on failure.</returns>
        [Tags("Company Employees")]
        [HttpDelete("{companyId:int}/employees/{employeeID:int}")]
        [HasPermission(Permission.CompanyEmployeeDelete)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmployee(
            [FromRoute] int companyId,
            [FromRoute] int employeeID,
            [FromQuery] bool force = false)
        {
            var result = await _companyService.DeleteEmployee(companyId, employeeID, force);
            return result.ToResponse();
        }

        /// <summary>
        /// Retrieves a paginated list of company employees for the authenticated CompanyAdmin.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong><br></br>
        /// Required permission: <strong>CompanyEmployeeRead</strong><br></br>
        /// <b>Paging and filtering parameters:</b><br/>
        /// <b>Sortable / Filterable Fields:</b>
        /// <ul>
        /// <li><c>ID</c></li>
        /// <li><c>CompanyID</c></li>
        /// <li><c>FirstName</c></li>
        /// <li><c>LastName</c></li>
        /// <li><c>Email</c></li>
        /// <li><c>VerificationStatus</c></li>
        /// <li><c>Role.Name</c></li>
        /// <li><c>ActiveStatus</c></li>
        /// <li><c>CreatedAt</c></li>
        /// <li><c>UpdatedAt</c></li>
        /// </ul>
        ///  Filtering Example: name~=Company 4||email~=Company40,id==3107
        /// </remarks>
        /// <param name="id">The ID of the company in the route.</param>
        /// <param name="parameters">Pagination and filtering parameters.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Paginated list of company employees or an error response.</returns>
        [Tags("Company Employees")]
        [HttpGet("{id:int}/employees")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanyEmployeeRead)]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<UserLoginDataDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RetrievePagedEmployees(
            [FromRoute] int id,
            [FromQuery] PagedParameters parameters,
            CancellationToken cancellationToken)
        {
            var result = await _companyService.RetrievePagedCompanyEmployees(id, parameters, cancellationToken);
            return result.ToResponse();
        }
        /// <summary>
        /// Updates/deletes media for the company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows you to update media for a company, including:
        /// - Adding new media
        /// - Updating existing media (including marking one image as the main media)
        /// - Removing media not included in the update list
        ///
        /// **Business rules:**
        /// - Exactly one media must be marked as the main image (`IsMain = true`) in the request.
        /// - if Empty list provided in the request will be removed all medias from the company.
        /// 
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanyMediaUpdate</strong>
        /// </remarks>
        /// <param name="id">The ID of the company to update media for.</param>
        /// <param name="mediaUpdates">A list of media update requests that include file uploads, changes to 'main' status, or removal instructions.</param>
        /// <returns>Result indicating success or failure of the operation.</returns>
        [Tags("Company Media")]
        [HttpPut("{id:int}/media")]
        [HasPermission(Permission.CompanyMediaUpdate)]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMedia([FromRoute] int id, [FromBody] List<UpdateCompanyMediaRequest> mediaUpdates)
        {
            var result = await _companyService.UpdateMedia(id, mediaUpdates);
            return result.ToResponse();
        }

        /// <summary>
        /// Creates branch for the company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows you to create branch for company:
        ///
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>BranchCreate</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company</param>
        /// <param name="request">the request body of the branch</param>
        /// <returns>Result indicating success or failure of the operation.</returns>
        [Tags("Company Branches")]
        [HttpPost("{companyId:int}/branches/")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.BranchCreate)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBranch([FromRoute] int companyId, [FromBody] BranchCreateRequest request)
        {
            var result = await _branchService.Create(companyId, request);
            return result.ToResponse();
        }

        /// <summary>
        /// Updates branch for the company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows you to update branch for company:
        ///
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>BranchUpdate</strong>
        /// </remarks>
        /// <param name="companyId">The ID of the company</param>
        /// <param name="branchId">The id of the branch </param>
        /// <param name="request">The request body of the branch </param>
        /// <returns>Result indicating success or failure of the operation.</returns>
        [Tags("Company Branches")]
        [HttpPut("{companyId:int}/branches/{branchId:int}")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.BranchUpdate)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBranch([FromRoute] int companyId, [FromRoute] int branchId, [FromBody] BranchUpdateRequest request)
        {
            var result = await _branchService.Update(companyId, branchId, request);
            return result.ToResponse();
        }
        /// <summary>
        /// Disable branch for the company.
        /// </summary>
        /// <remarks>
        /// This endpoint allows you to disable branch for a company
        ///
        /// **Business rules:**
        /// - company admin able to disable branch
        /// - after disabling company admin is not able to see that branch.
        /// - we are doing this because of avoid deletion of branch and releated bookings (for statistic), 
        /// because they are connected FK cascade delete.
        /// 
        /// Required role: <strong>CompanyAdmin, SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>BranchDisable</strong>
        /// </remarks>
        /// <param name="companyId">Company Id</param>
        /// <param name="branchId">Branch Id.</param>
        /// <returns>Result indicating success or failure of the operation.</returns>
        [Tags("Company Branches")]
        [HttpDelete("{companyId:int}/branches/{branchId:int}")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.BranchDisable)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBranch([FromRoute] int companyId, [FromRoute] int branchId)
        {
            var result = await _branchService.Delete(companyId, branchId, force: false);
            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves a paginated list of companies subscriptions.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanySubscriptionGet</strong><br/><br/>
        /// <b>Paging and filtering parameters:</b><br/>
        /// <b>Sortable / Filterable Fields:</b>
        /// <ul>
        /// <li><c>ID</c></li>
        /// <li><c>CompanyId</c></li>
        /// <li><c>SubscriptionPlanId</c></li>
        /// <li><c>StartDate</c></li>
        /// <li><c>EndDate</c></li>
        /// <li><c>Status</c></li>
        /// <li><c>AutoRenew</c></li>
        /// <li><c>CreatedAt</c></li>
        /// <li><c>UpdatedAt</c></li>
        /// </ul>
        /// Filtering Example: name~=Company 4||email~=Company40,id==3107
        /// </remarks>
        /// <param name="parameters">Pagination parameters including page number, size, and search filters.</param>
        /// <param name="cancellationToken">Request cancellation token.</param>
        /// <returns>Paged list of company subscription records.</returns>
        [Tags("Company Subscriptions")]
        [HttpGet("subscriptions")]
        [Logging(LoggingType.General)]
        [HasPermission(Permission.CompanySubscriptionGet)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<CompanyDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompanySubscriptionsRetrievePaged([FromQuery] PagedParameters parameters, CancellationToken cancellationToken)
        {
            var result = await _companySubscriptionService.RetrievePaged(parameters);

            return result.ToResponse();
        }
        /// <summary>
        /// Change subscription plan of a company.
        /// </summary>
        /// <remarks>
        /// Updates the subscription plan assigned to the company.
        /// 
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanySubscriptionUpdate</strong>
        /// </remarks>
        /// <param name="id">Company Id.</param>
        /// <param name="subscriptionPlanId">Subscription plan id.</param>
        [Tags("Company Subscriptions")]
        [HttpPut("{id:int}/subscription/plan/{subscriptionPlanId:int}")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanySubscriptionUpdate)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePlan(
            [FromRoute] int id,
            [FromRoute] int subscriptionPlanId)
        {
            var result = await _companySubscriptionService.ChangePlan(id, subscriptionPlanId);
            return result.ToResponse();
        }
        /// <summary>
        /// Activate or reactivate a company subscription.
        /// </summary>
        /// <remarks>
        /// Sets subscription period and marks it as Active.
        /// 
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanySubscriptionUpdate</strong>
        /// </remarks>
        [Tags("Company Subscriptions")]
        [HttpPost("{id:int}/subscription/activate")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanySubscriptionUpdate)]
        [ProducesResponseType(typeof(CompanySubscriptionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Activate(
            [FromRoute] int id,
            [FromBody] ActivateSubscriptionRequest request)
        {
            var result = await _companySubscriptionService
                .Activate(id, request);

            return result.ToResponse();
        }
        /// <summary>
        /// Cancel company subscription.
        /// </summary>
        /// <remarks>
        /// Cancels the subscription and disables auto-renew.
        /// 
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanySubscriptionUpdate</strong>
        /// </remarks>
        [Tags("Company Subscriptions")]
        [HttpPost("{id:int}/subscription/cancel")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanySubscriptionUpdate)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel([FromRoute] int id)
        {
            var result = await _companySubscriptionService.Cancel(id);
            return result.ToResponse();
        }
        /// <summary>
        /// Extend subscription period.
        /// </summary>
        /// <remarks>
        /// Extends subscription by specified number of months.
        /// 
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanySubscriptionUpdate</strong>
        /// </remarks>
        [Tags("Company Subscriptions")]
        [HttpPost("{id:int}/subscription/extend")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanySubscriptionUpdate)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Extend(
            [FromRoute] int id,
            [FromBody] ExtendSubscriptionRequest request)
        {
            var result = await _companySubscriptionService
                .Extend(id, request);

            return result.ToResponse();
        }
        /// <summary>
        /// Enable or disable auto-renew.
        /// </summary>
        /// <remarks>
        /// Sets auto-renew flag for subscription.
        /// 
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanySubscriptionUpdate</strong>
        /// </remarks>
        [Tags("Company Subscriptions")]
        [HttpPut("{id:int}/subscription/autorenew")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanySubscriptionUpdate)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetAutoRenew(
            [FromRoute] int id,
            [FromBody] SetAutoRenewRequest request)
        {
            var result = await _companySubscriptionService
                .SetAutoRenew(id, request);

            return result.ToResponse();
        }
        /// <summary>
        /// Update subscription period.
        /// </summary>
        /// <remarks>
        /// Manually updates the start and end dates of the company subscription.
        /// 
        /// Business rules:
        /// - Subscription must exist.
        /// - Subscription must be Active.
        /// - Start date must be earlier than end date.
        /// 
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>CompanySubscriptionUpdate</strong>
        /// </remarks>
        /// <param name="id">Company Id.</param>
        /// <param name="request">New subscription period.</param>
        [Tags("Company Subscriptions")]
        [HttpPut("{id:int}/subscription/period")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.CompanySubscriptionUpdate)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePeriod(
            [FromRoute] int id,
            [FromBody] UpdateSubscriptionPeriodRequest request)
        {
            var result = await _companySubscriptionService
                .UpdatePeriod(id, request);

            return result.ToResponse();
        }
    }
}
