using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.Common.ResultsErrors.Company;
using Application.Common.ResultsErrors.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class CompanyService :ICompanyService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly ICompanyInvitationRepository companyInvitationRepository;
        private readonly IConfiguration configuration;

        public CompanyService(
            ICompanyRepository companyRepository,
            IUserAccountRepository userAccountRepository,
            IHttpContextAccessor httpContextAccessor,
            IUserLoginDataRepository userLoginDataRepository,
            ICompanyInvitationRepository companyInvitationRepository,
            IConfiguration configuration)
        {
            this.companyRepository = companyRepository;
            this.userAccountRepository = userAccountRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.userLoginDataRepository = userLoginDataRepository;
            this.companyInvitationRepository = companyInvitationRepository;
            this.configuration = configuration;
        }

        public async Task<Result<string>> InviteMember(int memberID)
        {

            var authUserEmail = httpContextAccessor.HttpContext?.Items ["Email"] as string;
            if (authUserEmail == null)
                return Result.Failure<string>(AuthorizationDataErrors.NotFound);

            var authUserLoginData = await userLoginDataRepository.GetFullUserDataByEmail(authUserEmail);
            if (authUserLoginData == null)
                return Result.Failure<string>(AuthorizationDataErrors.NotFound);

            var member = await userAccountRepository.Get(memberID);
            if (member is null)
            {
                return Result.Failure<string>(InviteMemberErrors.NotFound);
            }
            if (authUserLoginData.UserAccount.Role!.ID != Role.CompanyAdmin.ID || member.RoleID != Role.User.ID)
            {
                return Result.Failure<string>(InviteMemberErrors.NotValidRole);
            }

            var expDays = Convert.ToDouble(configuration ["Jwt:VerificationTokenExpirationDays"]);
            var invitation = new CompanyInvitation()
            {
                CompanyId = (int)authUserLoginData.UserAccount.CompanyID!,
                MemberID = member.ID,
                Token = JWTGenerator.GenerateAndHashSecureToken(),
                ExpirationTime = DateTime.Now.AddDays(expDays),
                IsAccepted = false
            };

            await companyInvitationRepository.Add(invitation);

            return Result.Success(invitation.Token);
        }
        public async Task<Result> AcceptInvite(string token)
        {
            var authUserEmail = httpContextAccessor.HttpContext?.Items ["Email"] as string;
            if (authUserEmail == null)
                return Result.Failure(AuthorizationDataErrors.NotFound);

            var authUserLoginData = await userLoginDataRepository.GetFullUserDataByEmail(authUserEmail);
            if (authUserLoginData == null)
                return Result.Failure(AuthorizationDataErrors.NotFound);

            var invitation = await companyInvitationRepository.Get(token);
            if (invitation == null)
            {
                return Result.Failure(AcceptInviteErrors.NotFound);
            }
            if (invitation.MemberID != authUserLoginData.UserAccountID)
            {
                return Result.Failure(AcceptInviteErrors.InvalidUser);
            }
            if (invitation.ExpirationTime < DateTime.Now)
            {
                return Result.Failure(AcceptInviteErrors.TokenExpired);
            }

            invitation.IsAccepted = true;
            invitation.Token = string.Empty;
            invitation.UpdateTimestamp();
            await companyInvitationRepository.Update(invitation);

            authUserLoginData.UserAccount.CompanyID = invitation.CompanyId;
            authUserLoginData.UserAccount.RoleID = Role.CompanyMember.ID;
            authUserLoginData.UserAccount.UpdateTimestamp();
            await userAccountRepository.Update(authUserLoginData.UserAccount);

            return Result.Success();
        }
    }
}
