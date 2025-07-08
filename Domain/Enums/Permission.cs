namespace Domain.Enums
{
    public enum Permission
    {
        // ==== User Permissions ====
        UserCreate = 1,
        UserRead,
        UserUpdate,
        UserDelete,

        // ==== Company Permissions ====
        CompanyCreate,
        CompanyRead,
        CompanyUpdate,
        CompanyUpdatePartial,
        CompanyUpdateFull,
        CompanyDelete,
        CompanyReadAll,
        CompanyReadOwn,
        CompanyReadLimited,

        // ==== Company Member Permissions ====
        CompanyMemberCreate,
        CompanyMemberRead,
        CompanyMemberUpdate,
        CompanyMemberDelete,

        // ==== Report and Settings ====
        ReportView,
        SettingsManage,

        // ==== Work Schedule Permissions ====
        WorkScheduleCompanyRead,
        WorkScheduleCompanyCreate,
        WorkScheduleCompanyUpdate,
        WorkScheduleCompanyExceptionManage,

        WorkScheduleUserRead,
        WorkScheduleUserCreate,
        WorkScheduleUserUpdate,
        WorkScheduleUserExceptionManage,

        // ==== Service Permissions ====
        ServiceCreate,
        ServiceRead,
        ServiceUpdate,
        ServiceDelete,

        // ==== Appointment Permissions ====
        AppointmentSchedule,
        AppointmentRead,
        AppointmentUpdate,
        AppointmentCancel,
        AppointmentApprove,

        // ==== Media Permissions ====
        MediaUpload,
        MediaRead,
        MediaDelete,

        // ==== Company Media Permissions ====
        CompanyMediaUpload,
        CompanyMediaRead,
        CompanyMediaDelete,
        CompanyMediaUpdate,

        // ==== FAQ Permissions ====
        FaqCreate,
        FaqRead,
        FaqUpdate,
        FaqDelete,

        // ==== FAQ Category Permissions ====
        FaqCategoryCreate,
        FaqCategoryRead,
        FaqCategoryUpdate,
        FaqCategoryDelete,

        // ==== Invitation Permissions ====
        InvitationSend,
        InvitationRead,
        InvitationRevoke,

        // ==== Location / Geography Permissions ====
        LocationRead,
        CityRead,
        StateRead,
        CountryRead,

        // ==== Role and Permission Management ====
        RoleCreate,
        RoleRead,
        RoleUpdate,
        RoleDelete,
        RolePermissionManage,

        // ==== Auth & Login Permissions ====
        UserLoginDataRead,
        UserLoginDataManage
    }
}

