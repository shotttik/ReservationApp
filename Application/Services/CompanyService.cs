using Application.Authentication;
using Application.Common.Requests.Company;
using Application.Common.Results;
using Application.Common.Security;
using Application.Extensions;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO.Company;
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
        private readonly CompanyAccessGuard companyAccessGuard;

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
            CompanyAccessGuard companyAccessGuard)
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
            var AuthUser = await authService.GetCurrentUser();
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
            var AuthUser = await authService.GetCurrentUser();
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
            var errors = parameters.Validate<CompanyDTO>();
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

        public async Task<Result> UploadImages(UploadCompanyImagesRequest request, CancellationToken cancellationToken)
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
                    CompanyID = AuthUser.CompanyID!.Value,
                    MediaID = media.ID,
                    IsMain = item.IsMain
                };
                await companyMediaRepository.Add(companyMedia, cancellationToken);
            }

            return Result.Success(MediaResults.ImagesUploaded);
        }

        public async Task<Result> Update(CompanyPartialUpdateRequest request)
        {
            var authUser = await authService.GetCurrentUser();

            var company = await companyRepository.Get((int)authUser.CompanyID!);
            if (company == null) return Result.Failure(CompanyResults.CompanyDoesNotExists);
            company.Description = request.Description;

            await companyRepository.Update(company);

            return Result.Success();
        }
    }
}
