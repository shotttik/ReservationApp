using Application.Options;
using Domain.DTO.User;
using Domain.Entities.User;
using Domain.Enums;

namespace Application.Extensions.Mappers
{
    public static class UserLoginDataMapper
    {
        public static AuthUser MapToAuthorizationData(this UserLoginData user, AppUrls appUrls)
        {
            var baseUri = new Uri(appUrls.ApiBaseUrl);
            var media = user.UserAccount.UserAccountMedia.FirstOrDefault()?.Media;
            var webpUrl = BuildMediaUrl(baseUri, media?.RemoteUrl);
            var originalUrl = BuildMediaUrl(baseUri, media?.OriginalUrl);
            var userDTO = new AuthUser
            {
                Email = user.Email,
                Phone = user.Phone,
                Id = user.Id,
                UserAccountId = user.UserAccount.Id,
                FirstName = user.UserAccount.FirstName,
                LastName = user.UserAccount.LastName,
                Gender = user.UserAccount.Gender.HasValue ? (Gender?)user.UserAccount.Gender : null,
                DateOfBirth = user.UserAccount.DateOfBirth,
                ProfileImageUrlWebp = webpUrl,
                ProfileImageUrlOriginal = originalUrl,
                Role = new RoleDTO
                {
                    Id = user.UserAccount.Role!.ID,
                    Name = user.UserAccount.Role.Name,
                    Permissions = [.. user.UserAccount.Role.Permissions.Select(p => new PermissionDTO
                    {
                        Id = p.Id,
                        Name = p.Name
                    })]
                },
                CompanyId = user.UserAccount.CompanyID,
                BranchId = user.UserAccount.BranchId,
                WorkSchedules = [.. user.UserAccount.WorkSchedules.Select(e => e.MapToDTO())],
                Services = [.. user.UserAccount.EmployeeServices.Select(es => es.Service.MapToDTO())],
                ActiveStatus = user.ActiveStatus,
                CreatedAt = user.CreatedAt
            };

            return userDTO;
        }
        public static UserLoginDataDTO MapToDTO(this UserLoginData user, AppUrls appUrls)
        {
            var baseUri = new Uri(appUrls.ApiBaseUrl);
            var media = user.UserAccount.UserAccountMedia.FirstOrDefault()?.Media;
            var webpUrl = BuildMediaUrl(baseUri, media?.RemoteUrl);
            var originalUrl = BuildMediaUrl(baseUri, media?.OriginalUrl);

            var userDTO = new UserLoginDataDTO
            {
                Email = user.Email,
                Phone = user.Phone,
                Id = user.Id,
                FirstName = user.UserAccount.FirstName,
                LastName = user.UserAccount.LastName,
                Gender = user.UserAccount.Gender.HasValue ? (Gender?)user.UserAccount.Gender : null,
                DateOfBirth = user.UserAccount.DateOfBirth,
                BranchId = user.UserAccount.BranchId,
                ActiveStatus = user.ActiveStatus,
                EmailVerificationStatus = user.EmailVerificationStatus,
                PhoneVerificationStatus = user.PhoneVerificationStatus,
                ProfileImageUrlWebp = webpUrl,
                ProfileImageUrlOriginal = originalUrl,
                Role = new RoleDTO
                {
                    Id = user.UserAccount.Role!.ID,
                    Name = user.UserAccount.Role.Name,
                    Permissions = [.. user.UserAccount.Role.Permissions.Select(p => new PermissionDTO
                    {
                        Id = p.Id,
                        Name = p.Name
                    })]
                },
                CompanyId = user.UserAccount.CompanyID,
                WorkSchedules = user.UserAccount.WorkSchedules.Select(e => e.MapToDTO()).ToList(),
                Services = [.. user.UserAccount.EmployeeServices.Select(es => es.Service.MapToDTO())],
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return userDTO;
        }
        public static UserLoginDataDTO MapToDTO(this UserLoginData user)
        {
            var userDTO = new UserLoginDataDTO
            {
                Email = user.Email,
                Phone = user.Phone,
                Id = user.Id,
                FirstName = user.UserAccount.FirstName,
                LastName = user.UserAccount.LastName,
                Gender = user.UserAccount.Gender.HasValue ? (Gender?)user.UserAccount.Gender : null,
                DateOfBirth = user.UserAccount.DateOfBirth,
                BranchId = user.UserAccount.BranchId,
                ActiveStatus = user.ActiveStatus,
                EmailVerificationStatus = user.EmailVerificationStatus,
                PhoneVerificationStatus = user.PhoneVerificationStatus,
                Role = new RoleDTO
                {
                    Id = user.UserAccount.Role!.ID,
                    Name = user.UserAccount.Role.Name,
                    Permissions = [.. user.UserAccount.Role.Permissions.Select(p => new PermissionDTO
                    {
                        Id = p.Id,
                        Name = p.Name
                    })]
                },
                CompanyId = user.UserAccount.CompanyID,
                WorkSchedules = [.. user.UserAccount.WorkSchedules.Select(e => e.MapToDTO())],
                WorkScheduleExceptions = [.. user.UserAccount.WorkScheduleExceptions.Select(e => e.MapToDTO())],
                Services = [.. user.UserAccount.EmployeeServices.Select(e => e.Service.MapToDTO())],
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return userDTO;
        }

        private static string? BuildMediaUrl(Uri baseUri, string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return null;
            }

            return new Uri(baseUri, relativePath).ToString();
        }
    }
}
