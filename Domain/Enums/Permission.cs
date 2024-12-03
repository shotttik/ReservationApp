namespace Domain.Enums
{
    public enum Permission
    {
        // User Management Permissions
        AddUser = 1,
        EditUser = 2,
        DeleteUser = 3,
        UpdateUser = 4,

        // Company Management Permissions
        AddCompany = 5,
        EditCompany = 6,
        DeleteCompany = 7,

        // Report Viewing Permissions
        ViewReports = 8,

        // Settings Management Permissions
        ManageSettings = 9
    }
}
