using Domain.DTO.User;
using Domain.Entities.User;
using Domain.Enums;

namespace Application.Extensions.Mappers
{
    public static class UserAccountMapper
    {
        public static UserAccountDTO MapToDTO(this UserAccount userAccount)
        {
            return new UserAccountDTO()
            {
                ID = userAccount.ID,
                FirstName = userAccount.FirstName,
                LastName = userAccount.LastName,
                Gender = userAccount.Gender.HasValue ? (Gender?)userAccount.Gender : null,
                DateOfBirth = userAccount.DateOfBirth,
                Role = new RoleDTO
                {
                    ID = userAccount.Role!.ID,
                    Name = userAccount.Role.Name,
                    Permissions = userAccount.Role.Permissions.Select(p => new PermissionDTO
                    {
                        ID = p.ID,
                        Name = p.Name
                    }).ToList()
                },
                CompanyID = userAccount.CompanyID,
                WorkSchedules = userAccount.WorkSchedules.Select(e => e.MapToDTO()).ToList(),
            };
        }
    }
}
