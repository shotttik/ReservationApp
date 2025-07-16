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
        private readonly IAccessGuard companyAccessGuard;

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
            IAccessGuard companyAccessGuard)
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
            this.companyAccessGuard = companyAccessGuard;
        }

        public async Task<Result<string>> InviteMember(int memberID)
        {
            var AuthUser = await authService.GetCurrentUser();
            var member = await userAccountRepository.Get(memberID);
            if (member is null)
            {
                return Result.Failure<string>(CompanyResults.InviteMemberNotFound);
            }
            if (AuthUser.Role!.ID != Role.CompanyAdmin.ID || member.RoleID != Role.PublicUser.ID)
            {
                return Result.Failure<string>(CompanyResults.InviteInvalidRole);
            }
            await companyInvitationRepository.RevokePreviousInvite(memberID);

            var expDays = Convert.ToDouble(configuration ["Jwt:VerificationTokenExpirationDays"]);
            var invitation = new CompanyInvitation()
            {
                CompanyID = AuthUser.CompanyID!.Value,
                UserAccountID = member.ID,
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
            authUserEntity.RoleID = Role.CompanyMember.ID;

            await userAccountRepository.Update(authUserEntity);

            return Result.Success();
        }
        public async Task<Result> ServicesCreate(int routeCompanyId, ServicesCreateRequest request)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
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
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
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
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
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

        public async Task<Result> UploadMedia(int routeCompanyId, UploadCompanyImagesRequest request, CancellationToken cancellationToken)
        {
            var AuthUser = await authService.GetCurrentUser();
            var company = await companyRepository.Get(AuthUser.CompanyID!.Value);

            foreach (var item in request.Images)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var error = item.File.IsValidImage(configuration);
                if (error != Error.None)
                {
                    return Result.Failure(error);
                }
                var fileName = item.File.FileName;
                var contentType = item.File.ContentType;
                var fileStream = item.File.OpenReadStream();

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
                    FileSizeInBytes = item.File.Length
                };
                media = await mediaRepository.Add(media, cancellationToken);

                var companyMedia = new CompanyMedia()
                {
                    CompanyID = routeCompanyId,
                    MediaID = media.ID,
                    IsMain = item.IsMain
                };
                await companyMediaRepository.Add(companyMedia, cancellationToken);
            }

            return Result.Success(MediaResults.ImagesUploaded);
        }

        public async Task<Result> Update(int routeCompanyId, CompanyPartialUpdateRequest request)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
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

        public async Task<Result> CreateMember(int routeCompanyId, MemberCreateRequest request)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
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
                RoleID = (int)Domain.Enums.Role.CompanyMember,
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
        public async Task<Result> UpdateMember(int routeCompanyId, MemberUpdateRequest request)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
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
        public async Task<Result> DeleteMember(int routeCompanyId, int memberID, bool force)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            var userLoginData = await userLoginDataRepository.GetWithUserAccount(memberID, routeCompanyId);
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
                await userService.DeleteAllActiveSessions(memberID);
                return Result.Success(AuthResults.UserDeleted);

            }
            userLoginData.Disable();
            await userLoginDataRepository.Update(userLoginData);
            await userService.DeleteAllActiveSessions(memberID);
            return Result.Success(AuthResults.UserDisabled);
        }

        public async Task<Result<PagedList<UserLoginDataDTO>>> RetrievePagedCompanyMembers(int routeCompanyId, PagedParameters parameters, CancellationToken cancellationToken)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
            var userID = authService.GetUserLoginDataID();

            if (accessError != Error.None)
            {
                return Result.Failure<PagedList<UserLoginDataDTO>>(accessError);
            }
            var allowedFields = UserLoginDataFilterMap.DtoToEntityPath;
            var errors = parameters.Validate(allowedFields, typeof(UserLoginData));
            if (errors.Any())
            {
                return Result.Failure<PagedList<UserLoginDataDTO>>(PagedListResults.InvalidPagedParameters(errors.First()));
            }
            var users = await userLoginDataRepository.RetrievePagedCompanyMembers(parameters, cancellationToken, userID, routeCompanyId);

            return Result.Success(users);
        }
        public async Task<Result> UpdateMedia(int routeCompanyId, List<UpdateCompanyMediaRequest> mediaUpdates, CancellationToken cancellationToken)
        {
            var accessError = await companyAccessGuard.EnsureAccessToCompany(routeCompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            var company = await companyRepository.GetWithMedia(routeCompanyId);
            if (company == null)
                return Result.Failure(CompanyResults.CompanyDoesNotExists);
            // Process each media update request
            foreach (var mediaUpdate in mediaUpdates)
            {
                cancellationToken.ThrowIfCancellationRequested();
                // Handle uploading new images
                if (mediaUpdate.IsNewImage && mediaUpdate.File != null)
                {
                    var validImageError = mediaUpdate.File.IsValidImage(configuration);
                    if (validImageError != Error.None)
                    {
                        return Result.Failure(validImageError);
                    }

                    var fileStream = mediaUpdate.File.OpenReadStream();
                    var (originalPath, webpPath) = await fileStorageService.UploadWithWebp(
                        fileStream,
                        mediaUpdate.File.FileName,
                        mediaUpdate.File.ContentType,
                        Domain.Enums.UploadSubFolder.CompanyImages,
                        cancellationToken
                    );

                    // Create and save new media
                    var media = new Media
                    {
                        OriginalName = mediaUpdate.File.FileName,
                        RemoteUrl = webpPath,
                        FileType = mediaUpdate.File.ContentType,
                        FileSizeInBytes = mediaUpdate.File.Length
                    };

                    media = await mediaRepository.Add(media, cancellationToken);

                    // Add this new media to the company
                    var companyMedia = new CompanyMedia
                    {
                        CompanyID = routeCompanyId,
                        MediaID = media.ID,
                        IsMain = mediaUpdate.IsMain
                    };

                    await companyMediaRepository.Add(companyMedia, cancellationToken);
                }

                // Handle updating the main image
                if (mediaUpdate.IsMain)
                {
                    var currentMainMedia = company.CompanyMedias.Where(e => e.IsMain).ToList();
                    if (currentMainMedia.Count != 0)
                    {
                        foreach (var media in currentMainMedia)
                        {
                            media.IsMain = false;
                        }
                        await companyMediaRepository.UpdateRange(currentMainMedia, cancellationToken);
                    }

                    // Now mark the new media as the main image
                    var companyMediaToUpdate = company.CompanyMedias.FirstOrDefault(e => e.MediaID == mediaUpdate.MediaId);
                    if (companyMediaToUpdate != null)
                    {
                        companyMediaToUpdate.IsMain = true;
                        await companyMediaRepository.Update(companyMediaToUpdate, cancellationToken);
                    }
                }

                // Handle removing images
                if (mediaUpdate.IsRemoved)
                {
                    var companyMediaToDelete = company.CompanyMedias.FirstOrDefault(e => e.MediaID == mediaUpdate.MediaId);
                    if (companyMediaToDelete != null)
                    {
                        await companyMediaRepository.Delete(companyMediaToDelete, cancellationToken);
                        // Optionally, delete the file from storage
                        fileStorageService.Delete(companyMediaToDelete.Media.RemoteUrl);
                        await mediaRepository.Delete(companyMediaToDelete.Media, cancellationToken);
                    }
                }
            }

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
