using Domain.DTO.User;
using Domain.Entities.User;
using Domain.Enums;

namespace Application.Extensions.Mappers
{
    public static class UserLoginDataMapper
    {
        public static AuthUser MapToAuthorizationData(this UserLoginData user)
        {
            var userDTO = new AuthUser
            {
                Email = user.Email,
                Phone = user.Phone,
                Id = user.ID,
                UserAccountId = user.UserAccount.ID,
                FirstName = user.UserAccount.FirstName,
                LastName = user.UserAccount.LastName,
                Gender = user.UserAccount.Gender.HasValue ? (Gender?)user.UserAccount.Gender : null,
                DateOfBirth = user.UserAccount.DateOfBirth,
                ProfileImageUrl = user.UserAccount.UserAccountMedia.FirstOrDefault()?.Media.RemoteUrl,
                Role = new RoleDTO
                {
                    Id = user.UserAccount.Role!.ID,
                    Name = user.UserAccount.Role.Name,
                    Permissions = [.. user.UserAccount.Role.Permissions.Select(p => new PermissionDTO
                    {
                        Id = p.ID,
                        Name = p.Name
                    })]
                },
                CompanyId = user.UserAccount.CompanyID,
                BranchId = user.UserAccount.BranchId,
                WorkSchedules = [.. user.UserAccount.WorkSchedules.Select(e => e.MapToDTO())],
                CreatedAt = user.CreatedAt,

            };

            return userDTO;
        }
        public static UserLoginDataDTO MapToDTO(this UserLoginData user)
        {
            var userDTO = new UserLoginDataDTO
            {
                Email = user.Email,
                Phone = user.Phone,
                Id = user.ID,
                FirstName = user.UserAccount.FirstName,
                LastName = user.UserAccount.LastName,
                Gender = user.UserAccount.Gender.HasValue ? (Gender?)user.UserAccount.Gender : null,
                DateOfBirth = user.UserAccount.DateOfBirth,
                ActiveStatus = user.ActiveStatus,
                EmailVerificationStatus = user.EmailVerificationStatus,
                PhoneVerificationStatus = user.PhoneVerificationStatus,
                Role = new RoleDTO
                {
                    Id = user.UserAccount.Role!.ID,
                    Name = user.UserAccount.Role.Name,
                    Permissions = [.. user.UserAccount.Role.Permissions.Select(p => new PermissionDTO
                    {
                        Id = p.ID,
                        Name = p.Name
                    })]
                },
                CompanyId = user.UserAccount.CompanyID,
                WorkSchedules = user.UserAccount.WorkSchedules.Select(e => e.MapToDTO()).ToList(),
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return userDTO;
        }
    }
}
