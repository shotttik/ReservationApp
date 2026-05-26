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

        // ==== Company Employee Permissions ====
        CompanyEmployeeCreate,
        CompanyEmployeeRead,
        CompanyEmployeeUpdate,
        CompanyEmployeeDelete,

        // ==== Report and Settings ====
        ReportView,
        SettingsManage,

        // ==== Work Schedule Permissions ====

        WorkScheduleUserCreate,
        WorkScheduleUserUpdate,
        WorkScheduleUserDelete,
        WorkScheduleExceptionUserCreate,
        WorkScheduleExceptionUserUpdate,
        WorkScheduleExceptionUserDelete,


        // ==== Service Permissions ====
        ServiceCreate,
        ServiceRead,
        ServiceUpdate,
        ServiceDelete,

        // ==== Booking Permissions ====
        BookingCreate,
        BookingRead,
        BookingUpdate,
        BookingDelete,
        BookingCancel,
        BookingApprove,
        BookingResendCode,
        BookingVerify,

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

        // ==== Branch / Geography Permissions ====
        BranchRead,
        BranchCreate,
        BranchUpdate,
        BranchDelete,
        BranchDisable,
        BranchEnable,
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
        UserLoginDataManage,

        // ==== Review Permissions ====
        ReviewCreate,
        ReviewUpdate,
        ReviewInviteCreate,
        // ==== Subscription ====
        SubscriptionPlanCreate,
        SubscriptionPlanUpdate,
        SubscriptionPlanDelete,

        // ==== CompanySubscriptionUpdate ====
        CompanySubscriptionCreate,
        CompanySubscriptionUpdate,
        CompanySubscriptionDelete,
        CompanySubscriptionGet,

        // ==== Promo Code ====
        PromoCodeCreate,
        PromoCodeUpdate,
        PromoCodeDelete,
        PromoCodeRead
    }
}

