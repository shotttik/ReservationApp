IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Companies] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(255) NOT NULL,
    [Description] nvarchar(255) NULL,
    [IN] nvarchar(9) NOT NULL,
    [Email] nvarchar(255) NULL,
    [Phone] nvarchar(9) NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [Permissions] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [Roles] (
    [ID] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [RolePermissions] (
    [RoleID] int NOT NULL,
    [PermissionID] int NOT NULL,
    CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([RoleID], [PermissionID]),
    CONSTRAINT [FK_RolePermissions_Permissions_PermissionID] FOREIGN KEY ([PermissionID]) REFERENCES [Permissions] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_RolePermissions_Roles_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [Roles] ([ID]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserAccounts] (
    [ID] int NOT NULL IDENTITY,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [Gender] int NULL DEFAULT 5,
    [DateOfBirth] datetime2 NULL,
    [CompanyID] int NULL,
    [RoleID] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT [PK_UserAccounts] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_UserAccounts_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserAccounts_Roles_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [Roles] ([ID]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserLoginDatas] (
    [ID] int NOT NULL IDENTITY,
    [Email] nvarchar(255) NOT NULL,
    [PasswordHash] varbinary(255) NOT NULL,
    [PasswordSalt] varbinary(max) NOT NULL,
    [ConfirmationToken] nvarchar(150) NULL,
    [UserAccountID] int NOT NULL,
    [RefreshToken] nvarchar(max) NULL,
    [RefreshTokenExpirationTime] datetime2 NULL,
    [EmailValidationStatus] int NOT NULL DEFAULT 0,
    [PasswordRecoveryToken] nvarchar(150) NULL,
    [RecoveryTokenTime] datetime2 NULL,
    [UpdatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT [PK_UserLoginDatas] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_UserLoginDatas_UserAccounts_UserAccountID] FOREIGN KEY ([UserAccountID]) REFERENCES [UserAccounts] ([ID]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Name') AND [object_id] = OBJECT_ID(N'[Permissions]'))
    SET IDENTITY_INSERT [Permissions] ON;
INSERT INTO [Permissions] ([ID], [Name])
VALUES (1, N'AddUser'),
(2, N'EditUser'),
(3, N'DeleteUser'),
(4, N'UpdateUser'),
(5, N'AddCompany'),
(6, N'EditCompany'),
(7, N'DeleteCompany'),
(8, N'ViewReports'),
(9, N'ManageSettings');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Name') AND [object_id] = OBJECT_ID(N'[Permissions]'))
    SET IDENTITY_INSERT [Permissions] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Name') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([ID], [Name])
VALUES (1, N'SuperAdmin'),
(2, N'Admin'),
(3, N'User'),
(4, N'CompanyAdmin'),
(5, N'CompanyMember');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Name') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionID', N'RoleID') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
    SET IDENTITY_INSERT [RolePermissions] ON;
INSERT INTO [RolePermissions] ([PermissionID], [RoleID])
VALUES (1, 1),
(2, 1),
(3, 1),
(4, 1),
(5, 1),
(6, 1),
(7, 1),
(8, 1),
(9, 1),
(1, 2),
(2, 2),
(3, 2),
(5, 2),
(6, 2),
(7, 2),
(8, 2),
(8, 3),
(6, 4),
(7, 4);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionID', N'RoleID') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
    SET IDENTITY_INSERT [RolePermissions] OFF;
GO

CREATE INDEX [IX_RolePermissions_PermissionID] ON [RolePermissions] ([PermissionID]);
GO

CREATE INDEX [IX_UserAccounts_CompanyID] ON [UserAccounts] ([CompanyID]);
GO

CREATE INDEX [IX_UserAccounts_RoleID] ON [UserAccounts] ([RoleID]);
GO

CREATE UNIQUE INDEX [IX_UserLoginDatas_UserAccountID] ON [UserLoginDatas] ([UserAccountID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250322180618_initialMigration', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[UserLoginDatas].[EmailValidationStatus]', N'VerificationStatus', N'COLUMN';
GO

CREATE UNIQUE INDEX [IX_UserLoginDatas_Email] ON [UserLoginDatas] ([Email]);
GO

CREATE UNIQUE INDEX [IX_Companies_Email] ON [Companies] ([Email]) WHERE [Email] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Companies_IN] ON [Companies] ([IN]);
GO

CREATE UNIQUE INDEX [IX_Companies_Phone] ON [Companies] ([Phone]) WHERE [Phone] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250325181919_AddedUniqueIndexesForFieldsAndUpdatedVerificationStatus', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [UserLoginDatas] ADD [VerificationToken] nvarchar(150) NULL;
GO

ALTER TABLE [UserLoginDatas] ADD [VerificationTokenExpirationTime] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250326051332_AddedVerificationTokenInUserLoginData', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserLoginDatas]') AND [c].[name] = N'UpdatedAt');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [UserLoginDatas] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [UserLoginDatas] ALTER COLUMN [UpdatedAt] datetime2 NULL;
ALTER TABLE [UserLoginDatas] ADD DEFAULT (GETDATE()) FOR [UpdatedAt];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserLoginDatas]') AND [c].[name] = N'CreatedAt');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [UserLoginDatas] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [UserLoginDatas] ALTER COLUMN [CreatedAt] datetime2 NULL;
ALTER TABLE [UserLoginDatas] ADD DEFAULT (GETDATE()) FOR [CreatedAt];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserAccounts]') AND [c].[name] = N'UpdatedAt');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [UserAccounts] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [UserAccounts] ALTER COLUMN [UpdatedAt] datetime2 NULL;
ALTER TABLE [UserAccounts] ADD DEFAULT (GETDATE()) FOR [UpdatedAt];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserAccounts]') AND [c].[name] = N'CreatedAt');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [UserAccounts] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [UserAccounts] ALTER COLUMN [CreatedAt] datetime2 NULL;
ALTER TABLE [UserAccounts] ADD DEFAULT (GETDATE()) FOR [CreatedAt];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250326122406_fixedUpdatedAtField', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserLoginDatas]') AND [c].[name] = N'UpdatedAt');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [UserLoginDatas] DROP CONSTRAINT [' + @var4 + '];');
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserAccounts]') AND [c].[name] = N'UpdatedAt');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [UserAccounts] DROP CONSTRAINT [' + @var5 + '];');
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250326122907_fixedUpdatedAtFields', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [UserAccounts] DROP CONSTRAINT [FK_UserAccounts_Companies_CompanyID];
GO

ALTER TABLE [UserAccounts] ADD CONSTRAINT [FK_UserAccounts_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([ID]) ON DELETE SET NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250326123157_fixedCompanyDeletionSetNullToUserAccount', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Companies] ADD [CreatedAt] datetime2 NULL DEFAULT (GETDATE());
GO

ALTER TABLE [Companies] ADD [UpdatedAt] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250326123436_addedCompanyUpdateAndCreatedDateFields', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [CompanyInvitations] (
    [Id] int NOT NULL IDENTITY,
    [CompanyId] int NOT NULL,
    [MemberID] int NOT NULL,
    [Token] nvarchar(150) NOT NULL,
    [ExpirationTime] datetime2 NOT NULL,
    [IsAccepted] bit NOT NULL,
    [CreatedAt] datetime2 NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_CompanyInvitations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CompanyInvitations_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([ID]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_CompanyInvitations_CompanyId] ON [CompanyInvitations] ([CompanyId]);
GO

CREATE UNIQUE INDEX [IX_CompanyInvitations_Token] ON [CompanyInvitations] ([Token]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250404050925_AddedCompanyInvitationEntity', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserAccounts]') AND [c].[name] = N'DateOfBirth');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [UserAccounts] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [UserAccounts] ALTER COLUMN [DateOfBirth] date NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250404053201_ChangedDateOfBirthDataType', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[UserLoginDatas].[VerificationTokenExpirationTime]', N'VerificationTokenExpTime', N'COLUMN';
GO

EXEC sp_rename N'[UserLoginDatas].[RefreshTokenExpirationTime]', N'RefreshTokenExpTime', N'COLUMN';
GO

EXEC sp_rename N'[UserLoginDatas].[RecoveryTokenTime]', N'RecoveryTokenExpTime', N'COLUMN';
GO

EXEC sp_rename N'[UserLoginDatas].[PasswordRecoveryToken]', N'RecoveryToken', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250404055518_ModifiedExpirationTimesAndTokenNames', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [CompanyInvitations] DROP CONSTRAINT [FK_CompanyInvitations_Companies_CompanyId];
GO

EXEC sp_rename N'[CompanyInvitations].[CompanyId]', N'CompanyID', N'COLUMN';
GO

EXEC sp_rename N'[CompanyInvitations].[Id]', N'ID', N'COLUMN';
GO

EXEC sp_rename N'[CompanyInvitations].[MemberID]', N'UserAccountID', N'COLUMN';
GO

EXEC sp_rename N'[CompanyInvitations].[IX_CompanyInvitations_CompanyId]', N'IX_CompanyInvitations_CompanyID', N'INDEX';
GO

ALTER TABLE [CompanyInvitations] ADD CONSTRAINT [FK_CompanyInvitations_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([ID]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250408143902_MemberIDChangedToUserAccountID', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompanyInvitations]') AND [c].[name] = N'ExpirationTime');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [CompanyInvitations] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [CompanyInvitations] ALTER COLUMN [ExpirationTime] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250409121834_nullableVerificationTokenToCompanyInvitation', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP INDEX [IX_CompanyInvitations_Token] ON [CompanyInvitations];
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompanyInvitations]') AND [c].[name] = N'Token');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [CompanyInvitations] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [CompanyInvitations] ALTER COLUMN [Token] nvarchar(150) NULL;
GO

CREATE UNIQUE INDEX [IX_CompanyInvitations_Token] ON [CompanyInvitations] ([Token]) WHERE [Token] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250409122250_nullableVerificationTokenToCompanyInvitation1', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Services] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(255) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Duration] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [CompanyID] int NOT NULL,
    [CreatedAt] datetime2 NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Services] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Services_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([ID]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Services_CompanyID] ON [Services] ([CompanyID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250414115148_AddedServiceForCompany', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Appointments] (
    [ID] int NOT NULL IDENTITY,
    [ClientID] int NOT NULL,
    [EmployeeID] int NOT NULL,
    [CompanyID] int NULL,
    [StartTime] datetime2 NOT NULL,
    [EndTimeExpected] datetime2 NOT NULL,
    [EndTime] datetime2 NULL,
    [PriceExpected] decimal(18,2) NOT NULL,
    [PriceFull] decimal(18,2) NULL,
    [Discount] decimal(18,2) NULL,
    [PriceFinal] decimal(18,2) NULL,
    [Status] int NOT NULL DEFAULT 0,
    [CancellationReason] nvarchar(max) NULL,
    [CreatedAt] datetime2 NULL DEFAULT (GETDATE()),
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Appointments] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Appointments_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([ID]) ON DELETE SET NULL,
    CONSTRAINT [FK_Appointments_UserAccounts_ClientID] FOREIGN KEY ([ClientID]) REFERENCES [UserAccounts] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Appointments_UserAccounts_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [UserAccounts] ([ID]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Appointments_ClientID] ON [Appointments] ([ClientID]);
GO

CREATE INDEX [IX_Appointments_CompanyID] ON [Appointments] ([CompanyID]);
GO

CREATE INDEX [IX_Appointments_EmployeeID] ON [Appointments] ([EmployeeID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250415141129_addedAppointment', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [WorkingException] (
    [ID] int NOT NULL IDENTITY,
    [CompanyID] int NOT NULL,
    [UserAccountID] int NULL,
    [StartDateTime] datetime2 NOT NULL,
    [EndDateTime] datetime2 NOT NULL,
    [Reason] nvarchar(max) NULL,
    [IsFullDay] bit NOT NULL,
    CONSTRAINT [PK_WorkingException] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_WorkingException_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_WorkingException_UserAccounts_UserAccountID] FOREIGN KEY ([UserAccountID]) REFERENCES [UserAccounts] ([ID])
);
GO

CREATE TABLE [WorkingSchedule] (
    [ID] int NOT NULL IDENTITY,
    [CompanyID] int NOT NULL,
    [UserID] int NULL,
    [DayOfWeek] int NOT NULL,
    [StartTime] time NULL,
    [EndTime] time NULL,
    [IsWorkingDay] bit NOT NULL,
    CONSTRAINT [PK_WorkingSchedule] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_WorkingSchedule_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_WorkingSchedule_UserAccounts_UserID] FOREIGN KEY ([UserID]) REFERENCES [UserAccounts] ([ID])
);
GO

CREATE INDEX [IX_WorkingException_CompanyID] ON [WorkingException] ([CompanyID]);
GO

CREATE INDEX [IX_WorkingException_UserAccountID] ON [WorkingException] ([UserAccountID]);
GO

CREATE INDEX [IX_WorkingSchedule_CompanyID] ON [WorkingSchedule] ([CompanyID]);
GO

CREATE INDEX [IX_WorkingSchedule_UserID] ON [WorkingSchedule] ([UserID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250419210202_addedWorkingExceptionsForCOmpanyAndUsers', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [WorkingException] DROP CONSTRAINT [FK_WorkingException_Companies_CompanyID];
GO

ALTER TABLE [WorkingException] DROP CONSTRAINT [FK_WorkingException_UserAccounts_UserAccountID];
GO

ALTER TABLE [WorkingSchedule] DROP CONSTRAINT [FK_WorkingSchedule_Companies_CompanyID];
GO

ALTER TABLE [WorkingSchedule] DROP CONSTRAINT [FK_WorkingSchedule_UserAccounts_UserID];
GO

ALTER TABLE [WorkingSchedule] DROP CONSTRAINT [PK_WorkingSchedule];
GO

ALTER TABLE [WorkingException] DROP CONSTRAINT [PK_WorkingException];
GO

EXEC sp_rename N'[WorkingSchedule]', N'WorkingSchedules';
GO

EXEC sp_rename N'[WorkingException]', N'WorkingExceptions';
GO

EXEC sp_rename N'[WorkingSchedules].[IX_WorkingSchedule_UserID]', N'IX_WorkingSchedules_UserID', N'INDEX';
GO

EXEC sp_rename N'[WorkingSchedules].[IX_WorkingSchedule_CompanyID]', N'IX_WorkingSchedules_CompanyID', N'INDEX';
GO

EXEC sp_rename N'[WorkingExceptions].[IX_WorkingException_UserAccountID]', N'IX_WorkingExceptions_UserAccountID', N'INDEX';
GO

EXEC sp_rename N'[WorkingExceptions].[IX_WorkingException_CompanyID]', N'IX_WorkingExceptions_CompanyID', N'INDEX';
GO

ALTER TABLE [WorkingSchedules] ADD CONSTRAINT [PK_WorkingSchedules] PRIMARY KEY ([ID]);
GO

ALTER TABLE [WorkingExceptions] ADD CONSTRAINT [PK_WorkingExceptions] PRIMARY KEY ([ID]);
GO

ALTER TABLE [WorkingExceptions] ADD CONSTRAINT [FK_WorkingExceptions_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([ID]) ON DELETE CASCADE;
GO

ALTER TABLE [WorkingExceptions] ADD CONSTRAINT [FK_WorkingExceptions_UserAccounts_UserAccountID] FOREIGN KEY ([UserAccountID]) REFERENCES [UserAccounts] ([ID]) ON DELETE CASCADE;
GO

ALTER TABLE [WorkingSchedules] ADD CONSTRAINT [FK_WorkingSchedules_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([ID]) ON DELETE CASCADE;
GO

ALTER TABLE [WorkingSchedules] ADD CONSTRAINT [FK_WorkingSchedules_UserAccounts_UserID] FOREIGN KEY ([UserID]) REFERENCES [UserAccounts] ([ID]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250419212906_UpdatedAndAddedWorkingScheduleAndWorkingException', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [WorkingExceptions];
GO

DROP TABLE [WorkingSchedules];
GO

CREATE TABLE [WorkScheduleExceptions] (
    [ID] int NOT NULL IDENTITY,
    [CompanyID] int NOT NULL,
    [UserAccountID] int NULL,
    [StartDateTime] datetime2 NOT NULL,
    [EndDateTime] datetime2 NOT NULL,
    [Reason] nvarchar(max) NULL,
    [IsFullDay] bit NOT NULL,
    CONSTRAINT [PK_WorkScheduleExceptions] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_WorkScheduleExceptions_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_WorkScheduleExceptions_UserAccounts_UserAccountID] FOREIGN KEY ([UserAccountID]) REFERENCES [UserAccounts] ([ID]) ON DELETE CASCADE
);
GO

CREATE TABLE [WorkSchedules] (
    [ID] int NOT NULL IDENTITY,
    [CompanyID] int NOT NULL,
    [UserID] int NULL,
    [DayOfWeek] int NOT NULL,
    [StartTime] time NULL,
    [EndTime] time NULL,
    [IsWorkingDay] bit NOT NULL,
    CONSTRAINT [PK_WorkSchedules] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_WorkSchedules_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [Companies] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_WorkSchedules_UserAccounts_UserID] FOREIGN KEY ([UserID]) REFERENCES [UserAccounts] ([ID]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_WorkScheduleExceptions_CompanyID] ON [WorkScheduleExceptions] ([CompanyID]);
GO

CREATE INDEX [IX_WorkScheduleExceptions_UserAccountID] ON [WorkScheduleExceptions] ([UserAccountID]);
GO

CREATE INDEX [IX_WorkSchedules_CompanyID] ON [WorkSchedules] ([CompanyID]);
GO

CREATE INDEX [IX_WorkSchedules_UserID] ON [WorkSchedules] ([UserID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250420102302_updatedWorkScheduleTableNaming', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Name') AND [object_id] = OBJECT_ID(N'[Permissions]'))
    SET IDENTITY_INSERT [Permissions] ON;
INSERT INTO [Permissions] ([ID], [Name])
VALUES (10, N'ManageCompanyWorkSchedule'),
(11, N'ManageUserWorkSchedule');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Name') AND [object_id] = OBJECT_ID(N'[Permissions]'))
    SET IDENTITY_INSERT [Permissions] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionID', N'RoleID') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
    SET IDENTITY_INSERT [RolePermissions] ON;
INSERT INTO [RolePermissions] ([PermissionID], [RoleID])
VALUES (11, 3),
(10, 4);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionID', N'RoleID') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
    SET IDENTITY_INSERT [RolePermissions] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250423080556_AddedPermissionForWorkSchedule', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DELETE FROM [RolePermissions]
WHERE [PermissionID] = 11 AND [RoleID] = 3;
SELECT @@ROWCOUNT;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionID', N'RoleID') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
    SET IDENTITY_INSERT [RolePermissions] ON;
INSERT INTO [RolePermissions] ([PermissionID], [RoleID])
VALUES (11, 5);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionID', N'RoleID') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
    SET IDENTITY_INSERT [RolePermissions] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250423084909_AddedManageUserWorkSchedulePermisionForCompanyMember', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionID', N'RoleID') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
    SET IDENTITY_INSERT [RolePermissions] ON;
INSERT INTO [RolePermissions] ([PermissionID], [RoleID])
VALUES (11, 4);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionID', N'RoleID') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
    SET IDENTITY_INSERT [RolePermissions] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250424133312_AddedRolePermissionForCompanyAdmin', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [WorkSchedules] ADD [CreatedAt] datetime2 NULL DEFAULT (GETDATE());
GO

ALTER TABLE [WorkSchedules] ADD [UpdatedAt] datetime2 NULL;
GO

ALTER TABLE [WorkScheduleExceptions] ADD [CreatedAt] datetime2 NULL DEFAULT (GETDATE());
GO

ALTER TABLE [WorkScheduleExceptions] ADD [UpdatedAt] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250425121250_AddedMissingCreatedAdAndUpdatedAtFields', N'8.0.8');
GO

COMMIT;
GO


