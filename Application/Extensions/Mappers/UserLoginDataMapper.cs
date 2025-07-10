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
                ID = user.ID,
                FirstName = user.UserAccount.FirstName,
                LastName = user.UserAccount.LastName,
                Gender = user.UserAccount.Gender.HasValue ? (Gender?)user.UserAccount.Gender : null,
                DateOfBirth = user.UserAccount.DateOfBirth,
                Role = new RoleDTO
                {
                    ID = user.UserAccount.Role!.ID,
                    Name = user.UserAccount.Role.Name,
                    Permissions = user.UserAccount.Role.Permissions.Select(p => new PermissionDTO
                    {
                        ID = p.ID,
                        Name = p.Name
                    }).ToList()
                },
                CompanyID = user.UserAccount.CompanyID,
                WorkSchedules = user.UserAccount.WorkSchedules.Select(e => e.MapToDTO()).ToList(),
                CreatedAt = user.CreatedAt,

            };

            return userDTO;
        }
        public static UserLoginDataDTO MapToDTO(this UserLoginData user)
        {
            var userDTO = new UserLoginDataDTO
            {
                Email = user.Email,
                ID = user.ID,
                FirstName = user.UserAccount.FirstName,
                LastName = user.UserAccount.LastName,
                Gender = user.UserAccount.Gender.HasValue ? (Gender?)user.UserAccount.Gender : null,
                DateOfBirth = user.UserAccount.DateOfBirth,
                DeletedAt = user.DeletedAt,
                VerificationStatus = user.VerificationStatus,
                Role = new RoleDTO
                {
                    ID = user.UserAccount.Role!.ID,
                    Name = user.UserAccount.Role.Name,
                    Permissions = user.UserAccount.Role.Permissions.Select(p => new PermissionDTO
                    {
                        ID = p.ID,
                        Name = p.Name
                    }).ToList()
                },
                CompanyID = user.UserAccount.CompanyID,
                WorkSchedules = user.UserAccount.WorkSchedules.Select(e => e.MapToDTO()).ToList(),
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return userDTO;
        }
    }
}
