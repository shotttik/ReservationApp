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
            Create(Role.SuperAdmin, Permission.CompanyUpdateFull),
            Create(Role.SuperAdmin, Permission.CompanyDelete),
            Create(Role.SuperAdmin, Permission.CompanyReadAll),
            Create(Role.SuperAdmin, Permission.CompanyReadOwn),
            Create(Role.SuperAdmin, Permission.CompanyReadLimited),


            Create(Role.SuperAdmin, Permission.CompanyEmployeeCreate),
            Create(Role.SuperAdmin, Permission.CompanyEmployeeRead),
            Create(Role.SuperAdmin, Permission.CompanyEmployeeUpdate),
            Create(Role.SuperAdmin, Permission.CompanyEmployeeDelete),

            Create(Role.SuperAdmin, Permission.ReportView),
            Create(Role.SuperAdmin, Permission.SettingsManage),

            Create(Role.SuperAdmin, Permission.WorkScheduleUserCreate),
            Create(Role.SuperAdmin, Permission.WorkScheduleUserUpdate),
            Create(Role.SuperAdmin, Permission.WorkScheduleUserDelete),
            Create(Role.SuperAdmin, Permission.WorkScheduleExceptionUserCreate),
            Create(Role.SuperAdmin, Permission.WorkScheduleExceptionUserUpdate),
            Create(Role.SuperAdmin, Permission.WorkScheduleExceptionUserDelete),

            Create(Role.SuperAdmin, Permission.ServiceCreate),
            Create(Role.SuperAdmin, Permission.ServiceRead),
            Create(Role.SuperAdmin, Permission.ServiceUpdate),
            Create(Role.SuperAdmin, Permission.ServiceDelete),

            Create(Role.SuperAdmin, Permission.BookingCreate),
            Create(Role.SuperAdmin, Permission.BookingRead),
            Create(Role.SuperAdmin, Permission.BookingUpdate),
            Create(Role.SuperAdmin, Permission.BookingDelete),
            Create(Role.SuperAdmin, Permission.BookingCancel),
            Create(Role.SuperAdmin, Permission.BookingApprove),

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
            Create(Role.SuperAdmin, Permission.CompanyMediaUpdate),

            // ==== CompanyAdmin: Manage Own Company ====
            Create(Role.CompanyAdmin, Permission.CompanyRead),
            Create(Role.CompanyAdmin, Permission.CompanyReadOwn),
            Create(Role.CompanyAdmin, Permission.CompanyUpdate),
            Create(Role.CompanyAdmin, Permission.CompanyDelete),
            Create(Role.CompanyAdmin, Permission.CompanyUpdatePartial),

            Create(Role.CompanyAdmin, Permission.CompanyEmployeeCreate),
            Create(Role.CompanyAdmin, Permission.CompanyEmployeeRead),
            Create(Role.CompanyAdmin, Permission.CompanyEmployeeUpdate),
            Create(Role.CompanyAdmin, Permission.CompanyEmployeeDelete),

            Create(Role.CompanyAdmin, Permission.WorkScheduleUserCreate),
            Create(Role.CompanyAdmin, Permission.WorkScheduleUserUpdate),
            Create(Role.CompanyAdmin, Permission.WorkScheduleUserDelete),
            Create(Role.CompanyAdmin, Permission.WorkScheduleExceptionUserCreate),
            Create(Role.CompanyAdmin, Permission.WorkScheduleExceptionUserUpdate),
            Create(Role.CompanyAdmin, Permission.WorkScheduleExceptionUserDelete),

            Create(Role.CompanyAdmin, Permission.ServiceCreate),
            Create(Role.CompanyAdmin, Permission.ServiceRead),
            Create(Role.CompanyAdmin, Permission.ServiceUpdate),
            Create(Role.CompanyAdmin, Permission.ServiceDelete),

            Create(Role.CompanyAdmin, Permission.BookingCreate),
            Create(Role.CompanyAdmin, Permission.BookingRead),
            Create(Role.CompanyAdmin, Permission.BookingUpdate),
            Create(Role.CompanyAdmin, Permission.BookingCancel),
            Create(Role.CompanyAdmin, Permission.BookingApprove),

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
            Create(Role.CompanyAdmin, Permission.CompanyMediaUpdate),


            // ==== CompanyEmployee: Can Manage Own Schedule, View Services ====
            Create(Role.CompanyEmployee, Permission.WorkScheduleUserCreate),
            Create(Role.CompanyEmployee, Permission.WorkScheduleUserUpdate),
            Create(Role.CompanyEmployee, Permission.WorkScheduleUserDelete),
            Create(Role.CompanyEmployee, Permission.WorkScheduleExceptionUserCreate),
            Create(Role.CompanyEmployee, Permission.WorkScheduleExceptionUserUpdate),
            Create(Role.CompanyEmployee, Permission.WorkScheduleExceptionUserDelete),

            Create(Role.CompanyEmployee, Permission.ServiceRead),

            Create(Role.CompanyEmployee, Permission.BookingRead),
            Create(Role.CompanyEmployee, Permission.BookingUpdate),
            Create(Role.CompanyEmployee, Permission.BookingCancel),
            Create(Role.CompanyEmployee, Permission.BookingApprove),


            // ==== PublicUser: Can Only View Companies and Schedule Appointments ====
            Create(Role.PublicUser, Permission.CompanyReadLimited),
            Create(Role.PublicUser, Permission.ServiceRead),
            Create(Role.PublicUser, Permission.BookingRead),
            Create(Role.PublicUser, Permission.BookingUpdate)
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
