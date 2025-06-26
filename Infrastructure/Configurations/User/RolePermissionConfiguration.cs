using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Domain.Enums.Permission;

namespace Infrastructure.Configurations.User
{
    internal sealed class RolePermissionConfiguration
    :IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(x => new { x.RoleID, x.PermissionID });

            builder.HasData(
            // ==== SuperAdmin: Full Access ====
            Create(Role.SuperAdmin, Permission.UserCreate),
            Create(Role.SuperAdmin, Permission.UserRead),
            Create(Role.SuperAdmin, Permission.UserUpdate),
            Create(Role.SuperAdmin, Permission.UserDelete),

            Create(Role.SuperAdmin, Permission.CompanyCreate),
            Create(Role.SuperAdmin, Permission.CompanyRead),
            Create(Role.SuperAdmin, Permission.CompanyUpdate),
            Create(Role.SuperAdmin, Permission.CompanyDelete),

            Create(Role.SuperAdmin, Permission.ReportView),
            Create(Role.SuperAdmin, Permission.SettingsManage),

            Create(Role.SuperAdmin, Permission.WorkScheduleCompanyRead),
            Create(Role.SuperAdmin, Permission.WorkScheduleCompanyUpdate),
            Create(Role.SuperAdmin, Permission.WorkScheduleCompanyExceptionManage),

            Create(Role.SuperAdmin, Permission.WorkScheduleUserRead),
            Create(Role.SuperAdmin, Permission.WorkScheduleUserUpdate),
            Create(Role.SuperAdmin, Permission.WorkScheduleUserExceptionManage),

            Create(Role.SuperAdmin, Permission.ServiceCreate),
            Create(Role.SuperAdmin, Permission.ServiceRead),
            Create(Role.SuperAdmin, Permission.ServiceUpdate),
            Create(Role.SuperAdmin, Permission.ServiceDelete),

            Create(Role.SuperAdmin, Permission.AppointmentSchedule),
            Create(Role.SuperAdmin, Permission.AppointmentRead),
            Create(Role.SuperAdmin, Permission.AppointmentUpdate),
            Create(Role.SuperAdmin, Permission.AppointmentCancel),
            Create(Role.SuperAdmin, Permission.AppointmentApprove),

            Create(Role.SuperAdmin, Permission.RoleCreate),
            Create(Role.SuperAdmin, Permission.RoleRead),
            Create(Role.SuperAdmin, Permission.RoleUpdate),
            Create(Role.SuperAdmin, Permission.RoleDelete),
            Create(Role.SuperAdmin, Permission.RolePermissionManage),

            Create(Role.SuperAdmin, Permission.FaqCreate),
            Create(Role.SuperAdmin, Permission.FaqRead),
            Create(Role.SuperAdmin, Permission.FaqUpdate),
            Create(Role.SuperAdmin, Permission.FaqDelete),

            Create(Role.SuperAdmin, Permission.FaqCategoryCreate),
            Create(Role.SuperAdmin, Permission.FaqCategoryRead),
            Create(Role.SuperAdmin, Permission.FaqCategoryUpdate),
            Create(Role.SuperAdmin, Permission.FaqCategoryDelete),

            Create(Role.SuperAdmin, Permission.InvitationSend),
            Create(Role.SuperAdmin, Permission.InvitationRead),
            Create(Role.SuperAdmin, Permission.InvitationRevoke),

            Create(Role.SuperAdmin, Permission.CompanyMediaUpload),
            Create(Role.SuperAdmin, Permission.CompanyMediaRead),
            Create(Role.SuperAdmin, Permission.CompanyMediaDelete),

            // ==== CompanyAdmin: Manage Own Company ====
            Create(Role.CompanyAdmin, Permission.CompanyRead),
            Create(Role.CompanyAdmin, Permission.CompanyUpdate),
            Create(Role.CompanyAdmin, Permission.CompanyDelete),

            Create(Role.CompanyAdmin, Permission.WorkScheduleCompanyRead),
            Create(Role.CompanyAdmin, Permission.WorkScheduleCompanyCreate),
            Create(Role.CompanyAdmin, Permission.WorkScheduleCompanyUpdate),
            Create(Role.CompanyAdmin, Permission.WorkScheduleCompanyExceptionManage),

            Create(Role.CompanyAdmin, Permission.WorkScheduleUserRead),
            Create(Role.CompanyAdmin, Permission.WorkScheduleUserUpdate),
            Create(Role.CompanyAdmin, Permission.WorkScheduleUserCreate),
            Create(Role.CompanyAdmin, Permission.WorkScheduleUserExceptionManage),

            Create(Role.CompanyAdmin, Permission.ServiceCreate),
            Create(Role.CompanyAdmin, Permission.ServiceRead),
            Create(Role.CompanyAdmin, Permission.ServiceUpdate),
            Create(Role.CompanyAdmin, Permission.ServiceDelete),

            Create(Role.CompanyAdmin, Permission.AppointmentRead),
            Create(Role.CompanyAdmin, Permission.AppointmentApprove),

            Create(Role.CompanyAdmin, Permission.FaqCreate),
            Create(Role.CompanyAdmin, Permission.FaqRead),
            Create(Role.CompanyAdmin, Permission.FaqUpdate),
            Create(Role.CompanyAdmin, Permission.FaqDelete),

            Create(Role.CompanyAdmin, Permission.FaqCategoryCreate),
            Create(Role.CompanyAdmin, Permission.FaqCategoryRead),
            Create(Role.CompanyAdmin, Permission.FaqCategoryUpdate),
            Create(Role.CompanyAdmin, Permission.FaqCategoryDelete),

            Create(Role.CompanyAdmin, Permission.InvitationSend),
            Create(Role.CompanyAdmin, Permission.InvitationRead),
            Create(Role.CompanyAdmin, Permission.InvitationRevoke),

            Create(Role.CompanyAdmin, Permission.CompanyMediaUpload),
            Create(Role.CompanyAdmin, Permission.CompanyMediaRead),
            Create(Role.CompanyAdmin, Permission.CompanyMediaDelete),

            // ==== CompanyMember: Can Manage Own Schedule, View Services ====
            Create(Role.CompanyMember, Permission.WorkScheduleUserRead),
            Create(Role.CompanyMember, Permission.WorkScheduleUserCreate),
            Create(Role.CompanyMember, Permission.WorkScheduleUserUpdate),
            Create(Role.CompanyMember, Permission.WorkScheduleUserExceptionManage),

            Create(Role.CompanyMember, Permission.AppointmentSchedule),
            Create(Role.CompanyMember, Permission.AppointmentRead),
            Create(Role.CompanyMember, Permission.AppointmentCancel),

            Create(Role.CompanyMember, Permission.ServiceRead),

            // ==== PublicUser: Can Only View Companies and Schedule Appointments ====
            Create(Role.PublicUser, Permission.CompanyRead),
            Create(Role.PublicUser, Permission.ServiceRead),
            Create(Role.PublicUser, Permission.AppointmentSchedule),
            Create(Role.PublicUser, Permission.AppointmentRead)
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
