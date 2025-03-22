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
                   Create(Role.SuperAdmin, Permission.AddUser),
                Create(Role.SuperAdmin, Permission.EditUser),
                Create(Role.SuperAdmin, Permission.DeleteUser),
                Create(Role.SuperAdmin, Permission.AddCompany),
                Create(Role.SuperAdmin, Permission.UpdateUser),
                Create(Role.SuperAdmin, Permission.EditCompany),
                Create(Role.SuperAdmin, Permission.DeleteCompany),
                Create(Role.SuperAdmin, Permission.ViewReports),
                Create(Role.SuperAdmin, Permission.ManageSettings),
                Create(Role.Admin, Permission.AddUser),
                Create(Role.Admin, Permission.EditUser),
                Create(Role.Admin, Permission.DeleteUser),
                Create(Role.Admin, Permission.AddCompany),
                Create(Role.Admin, Permission.EditCompany),
                Create(Role.Admin, Permission.DeleteCompany),
                Create(Role.Admin, Permission.ViewReports),
                Create(Role.User, Permission.ViewReports),
                Create(Role.CompanyAdmin, Permission.EditCompany),
                Create(Role.CompanyAdmin, Permission.DeleteCompany)
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
