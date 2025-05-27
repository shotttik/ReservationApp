using Domain.DTO.User;
using Domain.DTO.WorkSchedule;
using Domain.Entities;
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
                Company = user.UserAccount.Company != null ? new UserCompanyDTO
                {
                    ID = user.UserAccount.Company.ID,
                    Name = user.UserAccount.Company.Name,
                    Description = user.UserAccount.Company.Description,
                    IN = user.UserAccount.Company.IN,
                    Email = user.UserAccount.Company.Email,
                    Phone = user.UserAccount.Company.Phone,
                    IsActive = user.UserAccount.Company.IsActive,
                    WorkSchedules = user.UserAccount.Company.WorkSchedules.Select(e => new WorkScheduleDTO
                    {
                        ID = e.ID,
                        CompanyID = e.CompanyID,
                        UserID = e.UserID,
                        DayOfWeek = e.DayOfWeek,
                        StartTime = e.StartTime,
                        EndTime = e.EndTime,
                        IsWorkingDay = e.IsWorkingDay
                    }).ToList()
                } : null,
                WorkSchedules = user.UserAccount.WorkSchedules.Select(e => new WorkScheduleDTO
                {
                    ID = e.ID,
                    CompanyID = e.CompanyID,
                    UserID = e.UserID,
                    DayOfWeek = e.DayOfWeek,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    IsWorkingDay = e.IsWorkingDay
                }).ToList(),
                CreatedAt = user.CreatedAt,

            };

            if (userDTO.Company != null)
            {
                userDTO.Company.WorkSchedules = userDTO.Company.WorkSchedules
                    .Where(w => w.UserID == null)
                    .ToList();
            }

            return userDTO;
        }
    }
}
