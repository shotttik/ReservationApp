using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.Common.ResultsErrors.Company;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class CompanyService :ICompanyService
    {
        private readonly IUserAccountRepository userAccountRepository;
        private readonly ICompanyInvitationRepository companyInvitationRepository;
        private readonly IConfiguration configuration;
        private readonly IAuthService authService;

        public CompanyService(
            ICompanyRepository companyRepository,
            IUserAccountRepository userAccountRepository,
            IHttpContextAccessor httpContextAccessor,
            IUserLoginDataRepository userLoginDataRepository,
            ICompanyInvitationRepository companyInvitationRepository,
            IConfiguration configuration,
            IAuthService authService)
        {
            this.userAccountRepository = userAccountRepository;
            this.companyInvitationRepository = companyInvitationRepository;
            this.configuration = configuration;
            this.authService = authService;
        }

        public async Task<Result<string>> InviteMember(int memberID)
        {
            var AuthUser = await authService.GetCurrentUser();
            var member = await userAccountRepository.Get(memberID);
            if (member is null)
            {
                return Result.Failure<string>(InviteMemberErrors.NotFound);
            }
            if (AuthUser.Role!.ID != Role.CompanyAdmin.ID || member.RoleID != Role.User.ID)
            {
                return Result.Failure<string>(InviteMemberErrors.NotValidRole);
            }
            await companyInvitationRepository.RevokePreviousInvite(memberID);

            var expDays = Convert.ToDouble(configuration ["Jwt:VerificationTokenExpirationDays"]);
            var invitation = new CompanyInvitation()
            {
                CompanyID = AuthUser.Company!.ID,
                UserAccountID = member.ID,
                Token = JWTGenerator.GenerateAndHashSecureToken(),
                ExpirationTime = DateTime.Now.AddDays(expDays),
                IsAccepted = false
            };

            await companyInvitationRepository.Add(invitation);

            return Result.Success(invitation.Token);
        }
        public async Task<Result> AcceptInvite(string token)
        {
            var AuthUser = await authService.GetCurrentUser();
            var invitation = await companyInvitationRepository.Get(token);
            if (invitation == null)
            {
                return Result.Failure(AcceptInviteErrors.NotFound);
            }
            if (invitation.UserAccountID != AuthUser.ID)
            {
                return Result.Failure(AcceptInviteErrors.InvalidUser);
            }
            if (invitation.ExpirationTime < DateTime.Now)
            {
                return Result.Failure(AcceptInviteErrors.TokenExpired);
            }

            invitation.IsAccepted = true;
            invitation.Token = null;
            invitation.ExpirationTime = null;
            invitation.UpdateTimestamp();
            await companyInvitationRepository.Update(invitation);

            var authUserEntity = await userAccountRepository.Get(AuthUser.ID);
            authUserEntity!.CompanyID = invitation.CompanyID;
            authUserEntity.RoleID = Role.CompanyMember.ID;
            authUserEntity.UpdateTimestamp();

            await userAccountRepository.Update(authUserEntity);

            return Result.Success();
        }
    }
}
