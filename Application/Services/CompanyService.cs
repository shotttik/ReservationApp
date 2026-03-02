using Application.Authentication;
using Application.Common.Requests;
using Application.Common.Requests.Company;
using Application.Common.Results;
using Application.Extensions;
using Application.Extensions.Mappers;
using Application.Extensions.Mappers.Pagination;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO.Company;
using Domain.DTO.User;
using Domain.Entities.Common;
using Domain.Entities.CompanyReleated;
using Domain.Entities.User;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class CompanyService :ICompanyService
    {
        private readonly IUserAccountRepository userAccountRepository;
        private readonly ICompanyInvitationRepository companyInvitationRepository;
        private readonly IConfiguration configuration;
        private readonly IAuthService authService;
        private readonly IServiceRepository serviceRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IFileStorageService fileStorageService;
        private readonly IMediaRepository mediaRepository;
        private readonly ICompanyMediaRepository companyMediaRepository;
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IUserService userService;
        private readonly IAccessGuard accessGuard;
        private readonly ISubscriptionGuard subscriptionGuard;

        public CompanyService(
            IUserAccountRepository userAccountRepository,
            ICompanyInvitationRepository companyInvitationRepository,
            IConfiguration configuration,
            IAuthService authService,
            IServiceRepository serviceRepository,
            ICompanyRepository companyRepository,
            IFileStorageService fileStorageService,
            IMediaRepository mediaRepository,
            ICompanyMediaRepository companyMediaRepository,
            IUserLoginDataRepository userLoginDataRepository,
            IUserService userService,
            IAccessGuard accessGuard,
            ISubscriptionGuard subscriptionGuard)
        {
            this.userAccountRepository = userAccountRepository;
            this.companyInvitationRepository = companyInvitationRepository;
            this.configuration = configuration;
            this.authService = authService;
            this.serviceRepository = serviceRepository;
            this.companyRepository = companyRepository;
            this.fileStorageService = fileStorageService;
            this.mediaRepository = mediaRepository;
            this.companyMediaRepository = companyMediaRepository;
            this.userLoginDataRepository = userLoginDataRepository;
            this.userService = userService;
            this.accessGuard = accessGuard;
            this.subscriptionGuard = subscriptionGuard;
        }

        public async Task<Result<string>> InviteEmployee(InviteEmployeeRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();
            var employee = await userAccountRepository.Get(request.UserAccountId);
            if (employee is null)
            {
                return Result.Failure<string>(CompanyResults.InviteEmployeeNotFound);
            }
            var subscriptionError = await subscriptionGuard.EnsureCanCreateEmployeeAsync(AuthUser.CompanyId!.Value);
            if (subscriptionError != Error.None)
            {
                return Result.Failure<string>(subscriptionError);
            }
            // must be used only for company admins , superadmin have different endpoint for this logic.
            if (employee.RoleID != Role.PublicUser.ID)
            {
                return Result.Failure<string>(CompanyResults.InviteInvalidRole);
            }
            var company = await companyRepository.GetWithBranches(AuthUser.CompanyId!.Value);
            if (company == null || !company.HasBranch(request.BranchId))
            {
                return Result.Failure<string>(CompanyResults.InvalidBranchId);
            }
            await companyInvitationRepository.RevokePreviousInvite(request.UserAccountId);

            var expDays = Convert.ToDouble(configuration ["Jwt:VerificationTokenExpirationDays"]);
            var invitation = new CompanyInvitation()
            {
                CompanyID = AuthUser.CompanyId!.Value,
                UserAccountID = employee.ID,
                Token = JWTGenerator.GenerateAndHashSecureToken(),
                ExpirationTime = DateTime.UtcNow.AddDays(expDays),
                IsAccepted = false,
                BranchId = request.BranchId
            };
            await companyInvitationRepository.Add(invitation);
            //TODO send email

            return Result.Success(invitation.Token);
        }
        public async Task<Result> InviteAccept(string token)
        {
            var AuthUser = await authService.GetCurrentUser();
            var invitation = await companyInvitationRepository.Get(token);
            if (invitation == null)
            {
                return Result.Failure(CompanyResults.InviteNotFound);
            }
            if (invitation.UserAccountID != AuthUser.UserAccountId)
            {
                return Result.Failure(CompanyResults.InviteInvalidUser);
            }
            if (invitation.ExpirationTime < DateTime.UtcNow)
            {
                return Result.Failure(CompanyResults.InviteTokenExpired);
            }

            invitation.IsAccepted = true;
            invitation.Token = null;
            invitation.ExpirationTime = null;
            await companyInvitationRepository.Update(invitation);

            var authUserEntity = await userAccountRepository.GetByUserLoginDataID(AuthUser.Id);
            authUserEntity!.CompanyID = invitation.CompanyID;
            authUserEntity.RoleID = Role.CompanyEmployee.ID;
            authUserEntity.BranchId = invitation.BranchId;

            await userAccountRepository.Update(authUserEntity);

            return Result.Success();
        }
        public async Task<Result> CreateServices(int routeCompanyId, ServicesCreateRequest request)
        {
            var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            var services = request.Services.Select(service => service.MapToEntity(routeCompanyId));
            await serviceRepository.AddRange(services);

            return Result.Success();
        }
        public async Task<Result> UpdateServices(int routeCompanyId, ServicesUpdateRequest request)
        {
            var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            var services = request.Services.Select(service => service.MapToEntity(routeCompanyId)).ToList();
            await serviceRepository.UpdateRange(services);

            return Result.Success();
        }
        public async Task<Result> DeleteServices(int routeCompanyId, int ID)
        {
            var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            var service = await serviceRepository.Get(ID);
            if (service == null || service.CompanyID != routeCompanyId)
            {
                return Result.Failure(CompanyResults.ServiceNotFound);
            }
            await serviceRepository.Delete(service);

            return Result.Success();
        }
        public async Task<Result<List<ServiceDTO>>> RetrieveServices(int routeCompanyId, bool forPublic)
        {
            if (!forPublic)
            {
                var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
                if (accessError != Error.None)
                {
                    return Result.Failure<List<ServiceDTO>>(accessError);
                }
            }
            var services = await serviceRepository.GetServicesByCompanyId(routeCompanyId, forPublic);
            var serviceDTOs = services.Select(s => s.MapToDTO()).ToList();

            return Result.Success(serviceDTOs);
        }
        public async Task<Result<PagedList<CompanyDTO>>> RetrievePaged(
           PagedParameters parameters,
           CancellationToken cancellationToken)
        {
            bool isSuperAdmin = authService.IsInRole(Role.SuperAdmin.Name);
            var allowedFields = CompanyFieldMap.DtoToEntityPath;
            var errors = parameters.Validate(allowedFields, typeof(CompanyDTO));
            if (errors.Any())
            {
                return Result.Failure<PagedList<CompanyDTO>>(PagedListResults.InvalidPagedParameters(errors.First()));
            }

            var companies = await companyRepository.RetrievePaged(
                parameters,
                cancellationToken,
                forPublic: isSuperAdmin
                );

            return companies;
        }

        public async Task<Result<CompanyDTO>> Get(int id)
        {
            var isSuperAdmin = authService.IsInRole(Role.SuperAdmin.Name);
            var company = isSuperAdmin ?
                await companyRepository.GetFullData(id)
                :await companyRepository.GetFullDataPublic(id);

            if (company is null)
            {
                return Result.Failure<CompanyDTO>(CompanyResults.CompanyNotFound);
            }

            return Result.Success(company.MapToDTO());
        }

        public async Task<Result<List<string>>> UploadMedia(int routeCompanyId, UploadCompanyMediaRequest request, CancellationToken cancellationToken)
        {
            var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure<List<string>>(accessError);
            }
            var company = await companyRepository.GetWithMedia(routeCompanyId);
            if (company == null)
                return Result.Failure<List<string>>(CompanyResults.CompanyDoesNotExists);

            var companyMediaEntities = new List<CompanyMedia>();
            var response = new List<string>();
            foreach (var item in request.Media)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var error = item.IsValidImage(configuration);
                if (error != Error.None)
                {
                    return Result.Failure<List<string>>(error);
                }
                var fileName = item.FileName;
                var contentType = item.ContentType;
                var fileStream = item.OpenReadStream();

                (string OriginalPath, string WebpPath) = await fileStorageService.UploadWithWebp(
                    fileStream,
                    fileName,
                    contentType,
                    Domain.Enums.UploadSubFolder.CompanyMedia,
                    cancellationToken);

                var media = new Media()
                {
                    OriginalName = fileName,
                    RemoteUrl = WebpPath,
                    FileType = contentType,
                    FileSizeInBytes = item.Length
                };
                await mediaRepository.Add(media, cancellationToken);
                response.Add(WebpPath);
                companyMediaEntities.Add(new CompanyMedia()
                {
                    CompanyID = routeCompanyId,
                    MediaID = media.ID,
                });
            }
            if (company.CompanyMedia.Count == 0) // tu media atvirtulia mashin update-s gamoikenben.
            {
                companyMediaEntities.First().IsMain = true;
            }
            await companyMediaRepository.AddOrUpdate(companyMediaEntities);

            return Result.Success(response);
        }

        public async Task<Result> Update(int routeCompanyId, CompanyPartialUpdateRequest request)
        {
            var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            var company = await companyRepository.Get(routeCompanyId);
            if (company == null) return Result.Failure(CompanyResults.CompanyDoesNotExists);

            company.ApplyPartialUpdate(request);

            await companyRepository.Update(company);

            return Result.Success();
        }

        public async Task<Result> CreateEmployee(int routeCompanyId, EmployeeCreateRequest request)
        {
            var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            var subscriptionError = await subscriptionGuard.EnsureCanCreateEmployeeAsync(routeCompanyId);
            if (subscriptionError != Error.None)
            {
                return Result.Failure<string>(subscriptionError);
            }
            var existingUser = await userLoginDataRepository.GetByEmail(request.Email);
            if (existingUser != null)
            {
                return Result.Failure(AuthResults.EmailAlreadyExists);
            }

            var verificationToken = JWTGenerator.GenerateAndHashSecureToken();
            var expDays = Convert.ToDouble(configuration ["Jwt:VerificationTokenExpirationDays"]);
            var verificationTokenExpirationTime = DateTime.UtcNow.AddDays(expDays);

            var userAccount = new UserAccount()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                RoleID = (int)Domain.Enums.Role.CompanyEmployee,
                CompanyID = routeCompanyId,
                BranchId = request.BranchId
            };

            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);

            var userLoginData = new UserLoginData()
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                EmailVerificationToken = verificationToken,
                EmailVerificationTokenExpTime = verificationTokenExpirationTime,
                UserAccount = userAccount
            };

            await userLoginDataRepository.Add(userLoginData);
            //TODO Send email

            return Result.Success(AuthResults.UserCreated);
        }
        public async Task<Result> UpdateEmployee(int routeCompanyId, EmployeeUpdateRequest request)
        {
            var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            var userAccount = await userAccountRepository.GetByUserLoginDataID(request.Id);
            if (userAccount is null || userAccount.CompanyID != routeCompanyId)
            {
                return Result.Failure(AuthResults.UserDoesntExists);
            }
            var company = await companyRepository.GetWithBranches(routeCompanyId);
            if (request.BranchId.HasValue)
            {
                if (company == null || !company.HasBranch((int)request.BranchId))
                {
                    return Result.Failure(CompanyResults.InvalidBranchId);
                }
                userAccount.BranchId = request.BranchId.Value;
            }
            if (request.FirstName is not null) userAccount.FirstName = request.FirstName;
            if (request.LastName is not null) userAccount.LastName = request.LastName;
            if (request.Gender.HasValue) userAccount.Gender = request.Gender.Value;
            if (request.DateOfBirth.HasValue) userAccount.DateOfBirth = request.DateOfBirth.Value;
            await userAccountRepository.Update(userAccount);
            await authService.RefreshUserCache(request.Id);

            return Result.Success(AuthResults.UserUpdated);
        }
        public async Task<Result> DeleteEmployee(int routeCompanyId, int employeeID, bool force)
        {
            var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            var userLoginData = await userLoginDataRepository.GetWithUserAccount(employeeID, routeCompanyId);
            if (userLoginData is null)
            {
                return Result.Failure(AuthResults.UserNotFound);
            }
            if (userLoginData.IsDisabled)
            {
                return Result.Failure(AuthResults.UserAlreadyDisabled);
            }
            if (force)
            {
                await userLoginDataRepository.Delete(userLoginData);
                await userService.DeleteAllActiveSessions(employeeID);
                return Result.Success(AuthResults.UserDeleted);

            }
            userLoginData.Disable();
            await userLoginDataRepository.Update(userLoginData);
            await userService.DeleteAllActiveSessions(employeeID);
            return Result.Success(AuthResults.UserDisabled);
        }

        public async Task<Result<PagedList<UserLoginDataDTO>>> RetrievePagedCompanyEmployees(int routeCompanyId, PagedParameters parameters, CancellationToken cancellationToken)
        {
            var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
            var userID = authService.GetUserLoginDataID();

            if (accessError != Error.None)
            {
                return Result.Failure<PagedList<UserLoginDataDTO>>(accessError);
            }
            var allowedFields = UserLoginDataFieldMap.DtoToEntityPath;
            var errors = parameters.Validate(allowedFields, typeof(UserLoginData));
            if (errors.Any())
            {
                return Result.Failure<PagedList<UserLoginDataDTO>>(PagedListResults.InvalidPagedParameters(errors.First()));
            }
            var users = await userLoginDataRepository.RetrievePagedCompanyEmployees(parameters, cancellationToken, userID, routeCompanyId);

            return Result.Success(users);
        }
        public async Task<Result> UpdateMedia(int routeCompanyId, List<UpdateCompanyMediaRequest> mediaUpdates)
        {
            var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            var mainCount = mediaUpdates.Count(m => m.IsMain);
            if (mediaUpdates.Count > 0 && mainCount != 1)
                return Result.Failure(CompanyResults.OnlyOneMainMedia);

            var company = await companyRepository.GetWithMedia(routeCompanyId);
            if (company == null)
                return Result.Failure(CompanyResults.CompanyDoesNotExists);

            var updateMediaIds = mediaUpdates.Select(m => m.MediaId).ToHashSet();
            var mediaExist = await mediaRepository.Exists(updateMediaIds);
            if (!mediaExist)
                return Result.Failure(MediaResults.SomeMediaDontExists);

            var toRemove = company.CompanyMedia
                .Where(cm => !updateMediaIds.Contains(cm.MediaID))
                .ToList();
            if (toRemove.Count != 0)
                await companyMediaRepository.DeleteRange(toRemove);

            var companyMediaEntities = mediaUpdates.Select(m => new CompanyMedia
            {
                CompanyID = routeCompanyId,
                MediaID = m.MediaId,
                IsMain = m.IsMain
            }).ToList();

            await companyMediaRepository.AddOrUpdate(companyMediaEntities);

            return Result.Success(MediaResults.ImagesUpdated);
        }

        public async Task<Result> ChangeActiveStatus(int routeCompanyId, ChangeStatusRequest request)
        {
            var company = await companyRepository.Get(routeCompanyId);

            if (company == null)
            {
                return Result.Failure(CompanyResults.CompanyNotFound);
            }

            if (company.ActiveStatus == request.NewStatus)
            {
                return Result.Failure(GenericResults.SameStatus);
            }
            if (request.NewStatus == Domain.Enums.ActiveStatus.Active)
            {
                company.Activate();
            }
            else
            {
                company.Disable();
            }
            await companyRepository.Update(company);

            return Result.Success(GenericResults.StatusChanged);
        }
    }
}
