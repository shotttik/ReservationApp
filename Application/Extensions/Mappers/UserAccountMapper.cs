using Application.DTOs.User;
using Application.DTOs.WorkSchedule;
using Domain.Entities;
using Domain.Enums;

namespace Application.Extensions.Mappers
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
                Gender = user.Gender.HasValue ? (Gender?)user.Gender : null,
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
                    Phone = user.Company.Phone,
                    WorkSchedules = user.Company.WorkSchedules.Select(e => new WorkScheduleDTO
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
                WorkSchedules = user.WorkSchedules.Select(e => new WorkScheduleDTO
                {
                    ID = e.ID,
                    CompanyID = e.CompanyID,
                    UserID = e.UserID,
                    DayOfWeek = e.DayOfWeek,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    IsWorkingDay = e.IsWorkingDay
                }).ToList()
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
