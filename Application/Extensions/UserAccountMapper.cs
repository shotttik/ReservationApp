using Application.DTOs.User;
using Domain.Entities;

namespace Application.Extensions
{
    public static class UserAccountMapper
    {
        public static UserAccountDTO MapToAuthorizationData(this UserAccount user)
        {
            var userDTO = new UserAccountDTO
            {
                ID = user.ID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Role = new RoleDTO
                {
                    ID = user.Role!.ID,
                    Name = user.Role.Name,
                    Permissions = user.Role.Permissions.Select(p => new PermissionDTO
                    {
                        ID = p.ID,
                        Name = p.Name
                    }).ToList()
                },
                Company = user.Company != null ? new CompanyDTO
                {
                    ID = user.Company.ID,
                    Name = user.Company.Name,
                    Description = user.Company.Description,
                    IN = user.Company.IN,
                    Email = user.Company.Email,
                    Phone = user.Company.Phone
                } : null
            };

            return userDTO;
        }
    }
}
