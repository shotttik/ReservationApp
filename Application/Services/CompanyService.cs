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
            IAccessGuard accessGuard)
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
        }

        public async Task<Result<string>> InviteEmployee(int employeeID)
        {
            var AuthUser = await authService.GetCurrentUser();
            var employee = await userAccountRepository.Get(employeeID);
            if (employee is null)
            {
                return Result.Failure<string>(CompanyResults.InviteEmployeeNotFound);
            }
            if (AuthUser.Role!.ID != Role.CompanyAdmin.ID || employee.RoleID != Role.PublicUser.ID)
            {
                return Result.Failure<string>(CompanyResults.InviteInvalidRole);
            }
            await companyInvitationRepository.RevokePreviousInvite(employeeID);

            var expDays = Convert.ToDouble(configuration ["Jwt:VerificationTokenExpirationDays"]);
            var invitation = new CompanyInvitation()
            {
                CompanyID = AuthUser.CompanyID!.Value,
                UserAccountID = employee.ID,
                Token = JWTGenerator.GenerateAndHashSecureToken(),
                ExpirationTime = DateTime.UtcNow.AddDays(expDays),
                IsAccepted = false
            };

            await companyInvitationRepository.Add(invitation);

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
            if (invitation.UserAccountID != authService.GetUserAccountID())
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

            var authUserEntity = await userAccountRepository.GetByUserLoginDataID(AuthUser.ID);
            authUserEntity!.CompanyID = invitation.CompanyID;
            authUserEntity.RoleID = Role.CompanyEmployee.ID;

            await userAccountRepository.Update(authUserEntity);

            return Result.Success();
        }
        public async Task<Result> ServicesCreate(int routeCompanyId, ServicesCreateRequest request)
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
        public async Task<Result> ServicesUpdate(int routeCompanyId, ServicesUpdateRequest request)
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
        public async Task<Result> ServicesDelete(int routeCompanyId, int ID)
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
        public async Task<Result<PagedList<CompanyDTO>>> RetrievePaged(
           PagedParameters parameters,
           CancellationToken cancellationToken,
           bool forPublic)
        {
            var allowedFields = CompanyFieldMap.DtoToEntityPath;
            var errors = parameters.Validate(allowedFields, typeof(CompanyDTO));
            if (errors.Any())
            {
                return Result.Failure<PagedList<CompanyDTO>>(PagedListResults.InvalidPagedParameters(errors.First()));
            }

            var companies = await companyRepository.RetrievePaged(
                parameters,
                cancellationToken,
                forPublic
                );

            return companies;
        }

        public async Task<Result<CompanyDTO>> Get(int id, bool forPublic)
        {
            var company = await companyRepository.GetFullData(id);
            if (company is null || (forPublic && !company.IsActive))
            {
                return Result.Failure<CompanyDTO>(CompanyResults.CompanyNotFound);
            }

            return Result.Success(company.MapToDTO());
        }

        public async Task<Result<List<int>>> UploadMedia(UploadCompanyMediasRequest request, CancellationToken cancellationToken)
        {
            var mediaIds = new List<int>();
            foreach (var item in request.Medias)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var error = item.IsValidImage(configuration);
                if (error != Error.None)
                {
                    return Result.Failure<List<int>>(error);
                }
                var fileName = item.FileName;
                var contentType = item.ContentType;
                var fileStream = item.OpenReadStream();

                (string OriginalPath, string WebpPath) = await fileStorageService.UploadWithWebp(
                    fileStream,
                    fileName,
                    contentType,
                    Domain.Enums.UploadSubFolder.CompanyImages,
                    cancellationToken);

                var media = new Media()
                {
                    OriginalName = fileName,
                    RemoteUrl = WebpPath,
                    FileType = contentType,
                    FileSizeInBytes = item.Length
                };
                await mediaRepository.Add(media, cancellationToken);
                mediaIds.Add(media.ID);
            }

            return Result.Success(mediaIds);
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
                CompanyID = routeCompanyId
            };

            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);

            var userLoginData = new UserLoginData()
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                VerificationToken = verificationToken,
                VerificationTokenExpTime = verificationTokenExpirationTime,
                UserAccount = userAccount
            };

            await userLoginDataRepository.Add(userLoginData);

            return Result.Success(AuthResults.UserCreated);
        }
        public async Task<Result> UpdateEmployee(int routeCompanyId, EmployeeUpdateRequest request)
        {
            var accessError = await accessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            var userAccount = await userAccountRepository.GetByUserLoginDataID(request.ID);
            if (userAccount is null || userAccount.CompanyID != routeCompanyId)
            {
                return Result.Failure(AuthResults.UserDoesntExists);
            }

            if (request.FirstName is not null) userAccount.FirstName = request.FirstName;
            if (request.LastName is not null) userAccount.LastName = request.LastName;
            if (request.Gender.HasValue) userAccount.Gender = request.Gender.Value;
            if (request.DateOfBirth.HasValue) userAccount.DateOfBirth = request.DateOfBirth.Value;
            await userAccountRepository.Update(userAccount);
            await authService.RefreshUserCache(request.ID);

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
            if (mainCount != 1)
                return Result.Failure(CompanyResults.OnlyOneMainMedia);

            var company = await companyRepository.GetWithMedia(routeCompanyId);
            if (company == null)
                return Result.Failure(CompanyResults.CompanyDoesNotExists);

            var updateMediaIds = mediaUpdates.Select(m => m.MediaId).ToHashSet();
            var mediasExist = await mediaRepository.Exists(updateMediaIds);
            if (!mediasExist)
                return Result.Failure(MediaResults.SomeMediaDontExists);

            var toRemove = company.CompanyMedias
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
