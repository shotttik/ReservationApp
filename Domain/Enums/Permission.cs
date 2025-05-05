namespace Domain.Enums
{
    public enum Permission
    {
        // User Permissions
        UserCreate = 1,
        UserRead = 2,
        UserUpdate = 3,
        UserDelete = 4,

        // Company Permissions
        CompanyCreate = 5,
        CompanyRead = 6,
        CompanyUpdate = 7,
        CompanyDelete = 8,

        // Report Permissions
        ReportView = 9,

        // Settings Permissions
        SettingsManage = 10,

        // WorkSchedule Permissions
        WorkScheduleManageCompany = 11,
        WorkScheduleManageUser = 12,

        // Service Permissions
        ServiceCreate = 13,
        ServiceRead = 14,
        ServiceUpdate = 15,
        ServiceDelete = 16,
    }
}
