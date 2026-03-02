using Application.Authentication;
using Application.Common.Requests.User;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Exceptions;
using Application.Extensions;
using Application.Extensions.Mappers;
using Application.Helpers;
using Application.Interfaces;
using Application.Options;
using Domain.DTO;
using Domain.DTO.User;
using Domain.Entities.Common;
using Domain.Entities.User;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shared.Utilities;
using Role = Domain.Entities.User.Role;

namespace Application.Services
{
    public class UserService :IUserService
    {
        private readonly IConfiguration configuration;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IAuthService authService;
        private readonly ICacheService cacheService;
        private readonly IFileStorageService fileStorageService;
        private readonly IMediaRepository mediaRepository;
        private readonly IUserAccountMediaRepository userAccountMediaRepository;
        private readonly IEmailTemplateBuilder emailBuilder;
        private readonly IMessageProducerService messageProducer;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AppUrls appUrls;

        public UserService(
           IConfiguration configuration,
           IUserAccountRepository userAccountRepository,
           IUserLoginDataRepository userLoginDataRepository,
           IHttpContextAccessor httpContextAccessor,
           IAuthService authService,
           ICacheService cacheService,
           IFileStorageService fileStorageService,
           IMediaRepository mediaRepository,
           IUserAccountMediaRepository userAccountMediaRepository,
           IEmailTemplateBuilder emailBuilder,
           IOptions<AppUrls> appUrls,
           IMessageProducerService messageProducer)
        {
            this.configuration = configuration;
            this.userAccountRepository = userAccountRepository;
            this.userLoginDataRepository = userLoginDataRepository;
            this.authService = authService;
            this.cacheService = cacheService;
            this.fileStorageService = fileStorageService;
            this.mediaRepository = mediaRepository;
            this.userAccountMediaRepository = userAccountMediaRepository;
            this.emailBuilder = emailBuilder;
            this.messageProducer = messageProducer;
            this.httpContextAccessor = httpContextAccessor;
            this.appUrls = appUrls.Value;
        }

        public async Task<Result> Register(RegisterUserRequest request)
        {
            if (await userLoginDataRepository.GetByEmail(request.Email) != null)
            {
                return Result.Failure(AuthResults.EmailAlreadyExists);
            }

            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);
            var verificationToken = JWTGenerator.GenerateAndHashSecureToken();
            var expDays = Convert.ToDouble(configuration ["Jwt:VerificationTokenExpirationDays"]);
            var verificationTokenExpirationTime = DateTime.UtcNow.AddDays(expDays);

            var userAccount = new UserAccount
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                RoleID = Role.PublicUser.ID,
            };

            var userLoginData = new UserLoginData
            {
                Email = request.Email,
                EmailVerificationStatus = VerificationStatus.Pending,
                PasswordHash = hash,
                PasswordSalt = salt,
                EmailVerificationToken = verificationToken,
                EmailVerificationTokenExpTime = verificationTokenExpirationTime,
                UserAccount = userAccount
            };

            await userLoginDataRepository.Add(userLoginData);
            var emailMessage = emailBuilder.BuildVerification(request.Email, request.FirstName, appUrls.VerificationLink);
            await messageProducer.PublishEmailAsync(emailMessage);

            return Result.Success(AuthResults.Registered);
        }
        public async Task<Result<LoginResponse>> Login(LoginRequest request)
        {
            var user = await userLoginDataRepository.GetFullUserDataByEmail(request.Email);

            if (user == null)
            {
                return Result.Failure<LoginResponse>(AuthResults.UserNotFound);
            }
            if (!user.IsEmailVerified)
            {
                return Result.Failure<LoginResponse>(AuthResults.EmailNotVerified);
            }
            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Result.Failure<LoginResponse>(AuthResults.InvalidPassword);
            }
            if (user.IsDisabled)
            {
                return Result.Failure<LoginResponse>(AuthResults.UserDisabledCantBeUsed);
            }

            var AuthUser = user.MapToAuthorizationData();
            var sessionInfo = SessionHelper.BuildSessionInfo(httpContextAccessor.HttpContext!, configuration, AuthUser);
            var accessToken = JWTGenerator.GenerateAccessToken(user.ID, user.UserAccount.ID, user.Email, sessionInfo.SessionId, configuration);

            await cacheService.SetAsync(
               CacheUtils.SessionKey(sessionInfo.SessionId),
               sessionInfo,
               sessionInfo.RefreshTokenExpTime - DateTime.UtcNow
            );

            var sessions = await cacheService.GetAsync<List<string>>(CacheUtils.ActiveSessionsKey(user.ID))
                ?? new List<string>();

            if (!sessions.Contains(sessionInfo.SessionId))
            {
                sessions.Add(sessionInfo.SessionId);
            }

            var expDays = Convert.ToDouble(configuration ["Jwt:UserActiveSessionsExpirationDays"]);
            var userActiveSessionsTTL = TimeSpan.FromDays(expDays);

            await cacheService.SetAsync(
                CacheUtils.ActiveSessionsKey(user.ID),
                sessions,
                userActiveSessionsTTL
            );

            return Result.Success(new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = sessionInfo.RefreshToken,
                AccessTokenExpTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration ["Jwt:AccessTokenExpirationMinutes"])),
            }, AuthResults.Success);
        }
        public async Task<Result<RefreshResponse>> Refresh(RefreshTokenRequest request)
        {
            // get access token from bearer
            var principal = JWTGenerator.GetPrincipalFromExpiredToken(request.AccessToken, configuration);
            if (principal == null)
            {
                return Result.Failure<RefreshResponse>(AuthResults.InvalidToken);
            }

            (string email, int userLoginDataID, int userAccountID, string sessionId) = JWTGenerator.ParseValuesFromPrincipal(principal);

            var session = await cacheService.GetAsync<SessionInfoDTO>(CacheUtils.SessionKey(sessionId));

            if (session is null ||
                session.RefreshToken is null ||
                session.RefreshToken != request.RefreshToken ||
                session.RefreshTokenExpTime < DateTime.UtcNow)
            {
                return Result.Failure<RefreshResponse>(AuthResults.InvalidToken);
            }
            var newAccessToken = JWTGenerator.GenerateAccessToken(userLoginDataID, userAccountID, email, sessionId, configuration);
            var newRefreshToken = JWTGenerator.GenerateAndHashSecureToken();
            var refreshTokenExpirationTime = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]));

            // Update session info
            session.LastAccessedAt = DateTime.UtcNow;
            session.RefreshToken = newRefreshToken;
            session.RefreshTokenExpTime = refreshTokenExpirationTime;

            await cacheService.SetAsync(CacheUtils.SessionKey(sessionId), session, refreshTokenExpirationTime - DateTime.UtcNow);

            var response = new RefreshResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };

            return Result.Success(response);
        }
        public async Task<Result> Logout()
        {
            UserAccountDTO AuthUser;
            try
            {
                AuthUser = await authService.GetCurrentUser();
            }
            catch (AuthorizationException)
            {
                return Result.Failure<UserAccountDTO>(AuthResults.NotAuthenticated);
            }
            var sessionID = authService.GetSessionID();

            if (string.IsNullOrWhiteSpace(sessionID))
                return Result.Failure(AuthResults.NotAuthenticated);

            var sessionInfo = await cacheService.GetAsync<SessionInfoDTO>(CacheUtils.SessionKey(sessionID));
            if (sessionInfo == null)
                return Result.Failure(AuthResults.NotAuthenticated);

            var userId = sessionInfo.AuthUser.Id;
            await cacheService.RemoveAsync(CacheUtils.SessionKey(sessionID));

            var sessionIds = await cacheService.GetAsync<List<string>>(CacheUtils.ActiveSessionsKey(userId)) ?? new List<string>();
            if (sessionIds.Remove(sessionID))
            {
                await cacheService.SetAsync(CacheUtils.ActiveSessionsKey(userId), sessionIds);
            }

            return Result.Success(AuthResults.Logouted);
        }
        public async Task<Result<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            var userLoginData = await userLoginDataRepository.GetByEmail(request.Email);
            if (userLoginData is null)
            {
                return Result.Success<string>(string.Empty, AuthResults.CheckEmail);
            }
            var recoveryToken = JWTGenerator.GenerateAndHashSecureToken();
            var recoveryTokenTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration ["Jwt:RecoveryTokenExpirationMinutes"]));
            userLoginData.RecoveryToken = recoveryToken;
            userLoginData.RecoveryTokenExpTime = recoveryTokenTime;
            await userLoginDataRepository.Update(userLoginData);

            // TODO instead of returning recovery token need to send email to user
            return Result.Success(recoveryToken, AuthResults.CheckEmail);
        }
        public async Task<Result> ResetPassword(ResetPasswordRequest request)
        {
            var userLoginData = await userLoginDataRepository.GetByRecoveryToken(request.RecoveryToken);

            if (userLoginData is null)
            {
                return Result.Failure(AuthResults.InvalidToken);
            }
            if (userLoginData.RecoveryTokenExpTime < DateTime.UtcNow)
            {
                return Result.Failure(AuthResults.TokenExpired);
            }

            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);
            userLoginData!.PasswordHash = hash;
            userLoginData.PasswordSalt = salt;
            userLoginData.RecoveryToken = null;
            userLoginData.RecoveryTokenExpTime = null;

            await userLoginDataRepository.Update(userLoginData);

            return Result.Success(AuthResults.PasswordReseted);
        }
        public async Task<Result<AuthUser>> GetUserAuthorizationData()
        {
            try
            {
                var AuthUser = await authService.GetCurrentUser();

                return Result.Success(AuthUser);
            }
            catch (AuthorizationException)
            {

                return Result.Failure<AuthUser>(AuthResults.NotAuthenticated);
            }
        }
        public async Task<Result> VerifyEmail(string token)
        {
            var userLoginData = await userLoginDataRepository.GetByEmailVerificationToken(token);
            if (userLoginData is null)
            {
                return Result.Failure(AuthResults.UserNotFound);
            }
            if (userLoginData.EmailVerificationTokenExpTime < DateTime.UtcNow)
            {
                return Result.Failure(AuthResults.TokenExpired);
            }

            if (!string.IsNullOrEmpty(userLoginData.PendingNewEmail))
            {
                userLoginData.Email = userLoginData.PendingNewEmail;
                userLoginData.PendingNewEmail = null;
            }
            else
            {
                userLoginData.EmailVerificationStatus = VerificationStatus.Verified;
            }
            userLoginData.EmailVerificationToken = null;
            userLoginData.EmailVerificationTokenExpTime = null;
            await userLoginDataRepository.Update(userLoginData);
            await DeleteAllActiveSessions(UserID: userLoginData.ID);

            return Result.Success(AuthResults.EmailVerified);
        }
        public async Task<Result> ChangeEmail(ChangeEmailRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();
            if (await userLoginDataRepository.GetByEmail(request.Email) != null)
            {
                return Result.Failure(AuthResults.EmailAlreadyExists);
            }
            var userLoginData = await userLoginDataRepository.Get(AuthUser.Id);
            if (userLoginData is null)
            {
                return Result.Failure(AuthResults.UserNotFound);
            }
            if (!userLoginData.IsEmailVerified)
            {
                return Result.Failure(AuthResults.EmailNotVerified);
            }
            if (userLoginData.EmailVerificationTokenExpTime != null && userLoginData.EmailVerificationTokenExpTime > DateTime.UtcNow)
            {
                return Result.Failure(AuthResults.EmailChangeAlreadyRequested);
            }
            var verificationToken = JWTGenerator.GenerateAndHashSecureToken();
            var expDays = Convert.ToDouble(configuration ["Jwt:VerificationTokenExpirationDays"]);
            var verificationTokenExpirationTime = DateTime.UtcNow.AddDays(expDays);

            userLoginData.PendingNewEmail = request.Email;
            userLoginData.EmailVerificationToken = verificationToken;
            userLoginData.EmailVerificationTokenExpTime = verificationTokenExpirationTime;

            await userLoginDataRepository.Update(userLoginData);
            var emailMessage = emailBuilder.BuildVerification(request.Email, AuthUser.FirstName, appUrls.VerificationLink);
            await messageProducer.PublishEmailAsync(emailMessage);

            return Result.Success(AuthResults.CheckEmail);
        }

        public async Task<Result> ChangePassword(ChangePasswordRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();
            var userLoginData = await userLoginDataRepository.Get(AuthUser.Id);
            if (!PasswordHasher.VerifyPassword(request.CurrentPassword, userLoginData!.PasswordHash, userLoginData.PasswordSalt))
            {
                return Result.Failure(AuthResults.InvalidPassword);
            }
            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);
            userLoginData.PasswordHash = hash;
            userLoginData.PasswordSalt = salt;
            await userLoginDataRepository.Update(userLoginData);

            return Result.Success(AuthResults.PasswordChanged);
        }

        public async Task<Result> Update(UpdateUserRequest request)
        {
            var userAccountID = authService.GetUserAccountID();
            var userAccount = await userAccountRepository.Get(userAccountID);
            if (request.ImageId != null)
            {
                var mediaExist = await mediaRepository.Exists([(int)request.ImageId]);
                if (!mediaExist)
                    return Result.Failure(MediaResults.SomeMediaDontExists);
                var userAccountMedia = new UserAccountMedia
                {
                    UserAccountId = userAccountID,
                    MediaId = (int)request.ImageId
                };
                await userAccountMediaRepository.EmptyThenAdd(userAccountMedia);
            }
            else
            {
                await userAccountMediaRepository.Empty(userAccountID);
            }
            userAccount!.FirstName = request.FirstName;
            userAccount!.LastName = request.LastName;
            userAccount.DateOfBirth = request.DateOfBirth;
            userAccount.Gender = request.Gender;
            await userAccountRepository.Update(userAccount);
            await authService.RefreshAuthUserCache();

            return Result.Success(AuthResults.UserUpdated);
        }

        public async Task<Result<List<SessionInfoSummaryDTO>>> GetActiveSessions()
        {
            var AuthUser = await authService.GetCurrentUser();
            var sessionIds = await cacheService.GetAsync<List<string>>(CacheUtils.ActiveSessionsKey(AuthUser.Id));

            var sessions = new List<SessionInfoSummaryDTO>();
            foreach (var sessionId in sessionIds!)
            {
                var sessionInfo = await cacheService.GetAsync<SessionInfoDTO>(CacheUtils.SessionKey(sessionId));
                if (sessionInfo != null)
                {
                    var sessionSummary = sessionInfo.MapToSummaryDTO();
                    if (sessionSummary.SessionId == authService.GetSessionID())
                    {
                        sessionSummary.IsCurrentSession = true;
                    }
                    sessions.Add(sessionSummary);
                }
            }

            return Result.Success(sessions);
        }

        public async Task<Result> DeleteActiveSession(string sessionId)
        {
            var AuthUser = await authService.GetCurrentUser();
            var sessionKey = CacheUtils.SessionKey(sessionId);
            var sessionInfo = await cacheService.GetAsync<SessionInfoDTO>(sessionKey);
            if (sessionInfo == null || sessionInfo.AuthUser.Id != AuthUser.Id)
            {
                return Result.Failure(AuthResults.SessionNotFound);
            }
            await cacheService.RemoveAsync(sessionKey);
            var sessionIds = await cacheService.GetAsync<List<string>>(CacheUtils.ActiveSessionsKey(AuthUser.Id)) ?? new List<string>();
            if (sessionIds.Remove(sessionId))
            {
                await cacheService.SetAsync(CacheUtils.ActiveSessionsKey(AuthUser.Id), sessionIds);
            }

            return Result.Success(AuthResults.SessionRemoved);
        }

        public async Task<Result> DeleteAllActiveSessions(int? UserID = null)
        {
            UserID ??= authService.GetUserLoginDataID();
            var activeSessionsKey = CacheUtils.ActiveSessionsKey((int)UserID);
            var sessionIds = await cacheService.GetAsync<List<string>>(activeSessionsKey);
            if (sessionIds == null || sessionIds.Count == 0)
            {
                return Result.Success(AuthResults.NoActiveSessions);
            }
            foreach (var sessionId in sessionIds)
            {
                await cacheService.RemoveAsync(CacheUtils.SessionKey(sessionId));
            }
            await cacheService.RemoveAsync(CacheUtils.ActiveSessionsKey((int)UserID));

            return Result.Success(AuthResults.AllSessionsRemoved);
        }

        // administrator can delete any user, otherwise only current user can delete their own account
        public async Task<Result> Delete(int? userID, bool force)
        {
            var invalidUserID = userID.HasValue && userID < 0;
            if (invalidUserID)
            {
                return Result.Failure(AuthResults.InvalidId);
            }
            userID ??= authService.GetUserLoginDataID();

            var userLoginData = await userLoginDataRepository.Get((int)userID);
            if (userLoginData == null)
            {
                return Result.Failure(AuthResults.UserDoesntExists);
            }
            if (userLoginData.IsDisabled)
            {
                return Result.Failure(AuthResults.UserAlreadyDisabled);
            }
            if (force)
            {
                await userLoginDataRepository.Delete(userLoginData);
                await DeleteAllActiveSessions(userID);
                return Result.Success(AuthResults.UserDeleted);
            }

            userLoginData.Disable();
            await userLoginDataRepository.Update(userLoginData);
            await DeleteAllActiveSessions(userID);

            return Result.Success(AuthResults.UserDisabled);
        }

        public async Task<Result<int>> UploadProfileImage(UploadUserProfileImageRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var error = request.File.IsValidImage(configuration);
            if (error != Error.None)
            {
                return Result.Failure<int>(error);
            }
            var fileName = request.File.FileName;
            var contentType = request.File.ContentType;
            var fileStream = request.File.OpenReadStream();

            (string OriginalPath, string WebpPath) = await fileStorageService.UploadWithWebp(
                fileStream,
                fileName,
                contentType,
                Domain.Enums.UploadSubFolder.UserProfileMedia,
                cancellationToken);

            var media = new Media()
            {
                OriginalName = fileName,
                RemoteUrl = WebpPath,
                FileType = contentType,
                FileSizeInBytes = request.File.Length
            };
            await mediaRepository.Add(media, cancellationToken);

            return Result.Success(media.ID);
        }
    }
}
