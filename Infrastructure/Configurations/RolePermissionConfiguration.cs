using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Domain.Enums.Permission;

namespace Infrastructure.Configurations
{
    internal sealed class RolePermissionConfiguration
    :IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(x => new { x.RoleID, x.PermissionID });

            builder.HasData(
                Create(Role.SuperAdmin, Permission.UserCreate),
                Create(Role.SuperAdmin, Permission.UserRead),
                Create(Role.SuperAdmin, Permission.UserDelete),
                Create(Role.SuperAdmin, Permission.UserUpdate),
                Create(Role.SuperAdmin, Permission.CompanyCreate),
                Create(Role.SuperAdmin, Permission.CompanyUpdate),
                Create(Role.SuperAdmin, Permission.CompanyRead),
                Create(Role.SuperAdmin, Permission.CompanyDelete),
                Create(Role.SuperAdmin, Permission.ReportView),
                Create(Role.SuperAdmin, Permission.SettingsManage),
                Create(Role.CompanyAdmin, Permission.CompanyUpdate),
                Create(Role.CompanyAdmin, Permission.CompanyDelete),
                Create(Role.CompanyAdmin, Permission.WorkScheduleManageCompany),
                Create(Role.CompanyAdmin, Permission.ServiceCreate),
                Create(Role.CompanyAdmin, Permission.ServiceUpdate),
                Create(Role.CompanyAdmin, Permission.ServiceRead),
                Create(Role.CompanyAdmin, Permission.ServiceDelete),
                Create(Role.CompanyAdmin, Permission.WorkScheduleManageUser),
                Create(Role.CompanyMember, Permission.WorkScheduleManageUser),
                Create(Role.PublicUser, Permission.CompanyRead)
                );
        }

        private static RolePermission Create(
        Role role, Permission permission)
        {
            return new RolePermission
            {
                RoleID = role.ID,
                PermissionID = (int)permission
            };
        }
    }
}
