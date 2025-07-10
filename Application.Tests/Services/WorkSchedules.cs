using Application.Common.Requests.WorkSchedule;
using Application.Common.Results;
using Application.Interfaces;
using Application.Services;
using Domain.DTO.User;
using Domain.Entities.Common;
using Domain.Entities.User;
using Domain.Interfaces.Repositories;
using Moq;

public class WorkScheduleServiceTests
{
    private readonly Mock<IWorkScheduleRepository> workScheduleRepositoryMock = new();
    private readonly Mock<IAuthService> authServiceMock = new();
    private readonly Mock<IUserAccountRepository> userAccountRepositoryMock = new();
    private readonly Mock<IAccessGuard> accessGuardMock = new();

    private readonly WorkScheduleService service;

    public WorkScheduleServiceTests()
    {
        service = new WorkScheduleService(
            workScheduleRepositoryMock.Object,
            userAccountRepositoryMock.Object,
            accessGuardMock.Object,
            authServiceMock.Object
        );
    }

    [Fact]
    public async Task CreateWorkScheduleAsync_ShouldReturnSuccess_WhenValidRequest()
    {
        // Arrange
        var userId = 1;
        var request = new WorkScheduleCreateRequest
        {
            UserID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        var userAccount = new UserAccount
        {
            ID = userId,
            CompanyID = 1,
            FirstName = "Test",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync(userAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Error.None);

        workScheduleRepositoryMock
            .Setup(x => x.Add(It.IsAny<WorkSchedule>()))
            .ReturnsAsync(new WorkSchedule());

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsSuccess);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Once);
    }

    [Fact]
    public async Task CreateWorkScheduleAsync_ShouldFail_WhenStartTimeAfterEndTime()
    {
        // Arrange
        var request = new WorkScheduleCreateRequest
        {
            UserID = 1,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(14, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.InvalidTimeRange, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Never);
    }

    [Fact]
    public async Task CreateWorkScheduleAsync_ShouldFail_WhenOverlappingScheduleExists()
    {
        // Arrange
        var userId = 1;
        var request = new WorkScheduleCreateRequest
        {
            UserID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(13, 0)
        };

        var existingSchedule = new WorkSchedule
        {
            ID = 2,
            UserAccountID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(11, 0)
        };

        var userAccount = new UserAccount
        {
            ID = userId,
            CompanyID = 1,
            FirstName = "Test",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule> { existingSchedule }
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync(userAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Error.None);

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.OverlappingSchedule, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Never);
    }

    [Fact]
    public async Task CreateWorkScheduleAsync_ShouldFail_WhenAccessDenied()
    {
        // Arrange
        var userId = 1;
        var request = new WorkScheduleCreateRequest
        {
            UserID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        var userAccount = new UserAccount
        {
            ID = userId,
            CompanyID = 1,
            FirstName = "Test",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync(userAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(GenericResults.Forbidden);

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Never);
    }
    [Fact]
    public async Task CreateWorkScheduleAsync_ShouldFail_WhenUserAccountNotFound()
    {
        // Arrange
        var userId = 1;
        var request = new WorkScheduleCreateRequest
        {
            UserID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync((UserAccount)null);

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.DontExists, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Never);
    }
    [Fact]
    public async Task CreateWorkScheduleAsync_ShouldFail_WhenStartTimeEqualsEndTime()
    {
        // Arrange
        var request = new WorkScheduleCreateRequest
        {
            UserID = 1,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(12, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.InvalidTimeRange, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Never);
    }
    [Fact]
    public async Task CreateWorkScheduleAsync_ShouldFail_WhenCompanyAdminCreatesScheduleForUserInAnotherCompany()
    {
        // Arrange
        var userId = 2; // target user
        var adminUser = new AuthUser
        {
            ID = 1,
            CompanyID = 1,
            Role = new RoleDTO { ID = (int)Domain.Enums.Role.CompanyAdmin, Name = "CompanyAdmin" },
            FirstName = "Admin",
            LastName = "User"
        };

        var targetUserAccount = new UserAccount
        {
            ID = userId,
            CompanyID = 2, // different company
            FirstName = "Target",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync(targetUserAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember((int)targetUserAccount.CompanyID!, userId))
            .ReturnsAsync(GenericResults.Forbidden);

        var request = new WorkScheduleCreateRequest
        {
            UserID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Never);
    }
    [Fact]
    public async Task CreateWorkSchedule_ShouldSucceed_WhenSuperUserCreatesScheduleForAnyUser()
    {
        // Arrange
        var userId = 2;
        var superUser = new AuthUser
        {
            ID = 1,
            CompanyID = null,
            Role = new RoleDTO { ID = (int)Domain.Enums.Role.SuperAdmin, Name = "SuperAdmin" },
            FirstName = "Super",
            LastName = "Admin"
        };

        var targetUserAccount = new UserAccount
        {
            ID = userId,
            CompanyID = 2,
            FirstName = "Target",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync(targetUserAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember((int)targetUserAccount.CompanyID!, userId))
            .ReturnsAsync(Error.None);

        workScheduleRepositoryMock
            .Setup(x => x.Add(It.IsAny<WorkSchedule>()))
            .ReturnsAsync(new WorkSchedule());

        var request = new WorkScheduleCreateRequest
        {
            UserID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsSuccess);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Once);
    }

    [Fact]
    public async Task CreateWorkSchedule_ShouldSucceed_WhenCompanyMemberCreatesOwnSchedule()
    {
        // Arrange
        var userId = 1;

        var userAccount = new UserAccount
        {
            ID = userId,
            CompanyID = 1,
            FirstName = "Member",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync(userAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember((int)userAccount.CompanyID!, userId))
            .ReturnsAsync(Error.None);

        workScheduleRepositoryMock
            .Setup(x => x.Add(It.IsAny<WorkSchedule>()))
            .ReturnsAsync(new WorkSchedule());

        var request = new WorkScheduleCreateRequest
        {
            UserID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsSuccess);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Once);
    }

    [Fact]
    public async Task CreateWorkSchedule_ShouldFail_WhenCompanyMemberCreatesScheduleForOtherUserInSameCompany()
    {
        // Arrange
        var userId = 2;

        var targetUserAccount = new UserAccount
        {
            ID = userId,
            CompanyID = 1,
            FirstName = "Other",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync(targetUserAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember((int)targetUserAccount.CompanyID!, userId))
            .ReturnsAsync(GenericResults.Forbidden);

        var request = new WorkScheduleCreateRequest
        {
            UserID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Never);
    }

    [Fact]
    public async Task CreateWorkSchedule_ShouldFail_WhenCompanyMemberCreatesScheduleForOtherUserInDifferentCompany()
    {
        // Arrange
        var userId = 2;

        var targetUserAccount = new UserAccount
        {
            ID = userId,
            CompanyID = 2,
            FirstName = "Other",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync(targetUserAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember((int)targetUserAccount.CompanyID!, userId))
            .ReturnsAsync(GenericResults.Forbidden);

        var request = new WorkScheduleCreateRequest
        {
            UserID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Never);
    }

    [Fact]
    public async Task UpdateWorkSchedule_ShouldFail_WhenScheduleNotFound()
    {
        // Arrange
        var request = new WorkScheduleUpdateRequest
        {
            ID = 1,
            UserID = 1,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(request.ID))
            .ReturnsAsync((WorkSchedule)null);

        // Act
        var result = await service.Update(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.DoesntExists, result.Error);
    }

    [Fact]
    public async Task UpdateWorkSchedule_ShouldFail_WhenStartTimeAfterEndTime()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 1,
            DayOfWeek = DayOfWeek.Monday
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        var request = new WorkScheduleUpdateRequest
        {
            ID = 1,
            UserID = 1,
            StartTime = new TimeOnly(13, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Update(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.InvalidTimeRange, result.Error);
    }

    [Fact]
    public async Task UpdateWorkSchedule_ShouldFail_WhenUserAccountNotFound()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 1,
            DayOfWeek = DayOfWeek.Monday
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(It.IsAny<int>()))
            .ReturnsAsync((UserAccount)null);

        var request = new WorkScheduleUpdateRequest
        {
            ID = 1,
            UserID = 1,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Update(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.DontExists, result.Error);
    }

    [Fact]
    public async Task UpdateWorkSchedule_ShouldFail_WhenUserAccountIdDoesNotMatchScheduleUserAccountId()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 2,
            DayOfWeek = DayOfWeek.Monday
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        var userAccount = new UserAccount
        {
            ID = 1,
            CompanyID = 1,
            FirstName = "Test",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userAccount.ID))
            .ReturnsAsync(userAccount);

        var request = new WorkScheduleUpdateRequest
        {
            ID = 1,
            UserID = 1,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Update(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
    }

    [Fact]
    public async Task UpdateWorkSchedule_ShouldFail_WhenAccessDenied()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 1,
            DayOfWeek = DayOfWeek.Monday
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        var userAccount = new UserAccount
        {
            ID = 1,
            CompanyID = 1,
            FirstName = "Test",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userAccount.ID))
            .ReturnsAsync(userAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(GenericResults.Forbidden);

        var request = new WorkScheduleUpdateRequest
        {
            ID = 1,
            UserID = 1,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Update(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
    }

    [Fact]
    public async Task UpdateWorkSchedule_ShouldFail_WhenOverlappingWithOtherSchedule()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 1,
            DayOfWeek = DayOfWeek.Monday
        };

        var otherSchedule = new WorkSchedule
        {
            ID = 2,
            UserAccountID = 1,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 0)
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        var userAccount = new UserAccount
        {
            ID = 1,
            CompanyID = 1,
            FirstName = "Test",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule> { otherSchedule }
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userAccount.ID))
            .ReturnsAsync(userAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Error.None);

        var request = new WorkScheduleUpdateRequest
        {
            ID = 1,
            UserID = 1,
            StartTime = new TimeOnly(11, 0),
            EndTime = new TimeOnly(13, 0)
        };

        // Act
        var result = await service.Update(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.OverlappingSchedule, result.Error);
    }

    [Fact]
    public async Task UpdateWorkSchedule_ShouldReturnSuccess_WhenValid()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 1,
            DayOfWeek = DayOfWeek.Monday
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        var userAccount = new UserAccount
        {
            ID = 1,
            CompanyID = 1,
            FirstName = "Test",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userAccount.ID))
            .ReturnsAsync(userAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Error.None);

        var request = new WorkScheduleUpdateRequest
        {
            ID = 1,
            UserID = 1,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Update(request);

        // Assert
        Assert.True(result.IsSuccess);
        workScheduleRepositoryMock.Verify(x => x.Update(It.IsAny<WorkSchedule>()), Times.Once);
    }
    [Fact]
    public async Task UpdateWorkSchedule_ShouldFail_WhenCompanyAdminUpdatesScheduleForUserInAnotherCompany()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 2,
            DayOfWeek = DayOfWeek.Monday
        };

        var targetUserAccount = new UserAccount
        {
            ID = 2,
            CompanyID = 2, // different company
            FirstName = "Target",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(targetUserAccount.ID))
            .ReturnsAsync(targetUserAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember((int)targetUserAccount.CompanyID!, targetUserAccount.ID))
            .ReturnsAsync(GenericResults.Forbidden);

        var request = new WorkScheduleUpdateRequest
        {
            ID = schedule.ID,
            UserID = targetUserAccount.ID,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Update(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Update(It.IsAny<WorkSchedule>()), Times.Never);
    }
    [Fact]
    public async Task UpdateWorkSchedule_ShouldFail_WhenCompanyMemberUpdatesOtherUserInAnotherCompany()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 2,
            DayOfWeek = DayOfWeek.Monday
        };

        var targetUserAccount = new UserAccount
        {
            ID = 2,
            CompanyID = 2, // different company
            FirstName = "Target",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(targetUserAccount.ID))
            .ReturnsAsync(targetUserAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember((int)targetUserAccount.CompanyID!, targetUserAccount.ID))
            .ReturnsAsync(GenericResults.Forbidden);

        var request = new WorkScheduleUpdateRequest
        {
            ID = schedule.ID,
            UserID = targetUserAccount.ID,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Update(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Update(It.IsAny<WorkSchedule>()), Times.Never);
    }
    [Fact]
    public async Task UpdateWorkSchedule_ShouldSucceed_WhenSuperUserUpdatesScheduleForAnyUser()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 2,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(9, 0)
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        var targetUserAccount = new UserAccount
        {
            ID = 2,
            CompanyID = 2,
            FirstName = "Target",
            LastName = "User",
            RoleID = Role.CompanyMember.ID,
            WorkSchedules = new List<WorkSchedule>()
        };

        userAccountRepositoryMock
            .Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(targetUserAccount.ID))
            .ReturnsAsync(targetUserAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember((int)targetUserAccount.CompanyID!, targetUserAccount.ID))
            .ReturnsAsync(Error.None);

        var request = new WorkScheduleUpdateRequest
        {
            ID = schedule.ID,
            UserID = targetUserAccount.ID,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Update(request);

        // Assert
        Assert.True(result.IsSuccess);
        workScheduleRepositoryMock.Verify(x => x.Update(It.IsAny<WorkSchedule>()), Times.Once);
    }

    [Fact]
    public async Task DeleteWorkSchedule_ShouldFail_WhenScheduleNotFound()
    {
        // Arrange
        workScheduleRepositoryMock
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync((WorkSchedule)null);

        // Act
        var result = await service.Delete(1);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.DoesntExists, result.Error);
    }

    [Fact]
    public async Task DeleteWorkSchedule_ShouldFail_WhenUserAccountNotFound()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 1
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        userAccountRepositoryMock
            .Setup(x => x.Get(schedule.UserAccountID))
            .ReturnsAsync((UserAccount)null);

        // Act
        var result = await service.Delete(schedule.ID);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
    }

    [Fact]
    public async Task DeleteWorkSchedule_ShouldFail_WhenAccessDenied()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 1
        };

        var userAccount = new UserAccount
        {
            ID = 1,
            CompanyID = 1,
            UserLoginDataID = 1,
            FirstName = "Test",
            LastName = "User",
            RoleID = Role.CompanyMember.ID
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        userAccountRepositoryMock
            .Setup(x => x.Get(schedule.UserAccountID))
            .ReturnsAsync(userAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(GenericResults.Forbidden);

        // Act
        var result = await service.Delete(schedule.ID);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
    }

    [Fact]
    public async Task DeleteWorkSchedule_ShouldReturnSuccess_WhenValid()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 1
        };

        var userAccount = new UserAccount
        {
            ID = 1,
            CompanyID = 1,
            UserLoginDataID = 1,
            FirstName = "Test",
            LastName = "User",
            RoleID = Role.CompanyMember.ID
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        userAccountRepositoryMock
            .Setup(x => x.Get(schedule.UserAccountID))
            .ReturnsAsync(userAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Error.None);

        // Act
        var result = await service.Delete(schedule.ID);

        // Assert
        Assert.True(result.IsSuccess);
        workScheduleRepositoryMock.Verify(x => x.Delete(schedule), Times.Once);
    }
    [Fact]
    public async Task DeleteWorkSchedule_ShouldFail_WhenCompanyAdminDeletesScheduleForUserInAnotherCompany()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 2
        };

        var targetUserAccount = new UserAccount
        {
            ID = 2,
            CompanyID = 2, // different company
            UserLoginDataID = 2,
            FirstName = "Target",
            LastName = "User",
            RoleID = Role.CompanyMember.ID
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        userAccountRepositoryMock
            .Setup(x => x.Get(schedule.UserAccountID))
            .ReturnsAsync(targetUserAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember((int)targetUserAccount.CompanyID!, targetUserAccount.UserLoginDataID))
            .ReturnsAsync(GenericResults.Forbidden);

        // Act
        var result = await service.Delete(schedule.ID);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Delete(It.IsAny<WorkSchedule>()), Times.Never);
    }
    [Fact]
    public async Task DeleteWorkSchedule_ShouldFail_WhenCompanyMemberDeletesOtherUserInAnotherCompany()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 2
        };

        var targetUserAccount = new UserAccount
        {
            ID = 2,
            CompanyID = 2, // different company
            UserLoginDataID = 2,
            FirstName = "Target",
            LastName = "User",
            RoleID = Role.CompanyMember.ID
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        userAccountRepositoryMock
            .Setup(x => x.Get(schedule.UserAccountID))
            .ReturnsAsync(targetUserAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember((int)targetUserAccount.CompanyID!, targetUserAccount.UserLoginDataID))
            .ReturnsAsync(GenericResults.Forbidden);

        // Act
        var result = await service.Delete(schedule.ID);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
        workScheduleRepositoryMock.Verify(x => x.Delete(It.IsAny<WorkSchedule>()), Times.Never);
    }
    [Fact]
    public async Task DeleteWorkSchedule_ShouldSucceed_WhenSuperUserDeletesScheduleForAnyUser()
    {
        // Arrange
        var schedule = new WorkSchedule
        {
            ID = 1,
            UserAccountID = 2
        };

        var targetUserAccount = new UserAccount
        {
            ID = 2,
            CompanyID = 2,
            UserLoginDataID = 2,
            FirstName = "Target",
            LastName = "User",
            RoleID = Role.CompanyMember.ID
        };

        workScheduleRepositoryMock
            .Setup(x => x.Get(schedule.ID))
            .ReturnsAsync(schedule);

        userAccountRepositoryMock
            .Setup(x => x.Get(schedule.UserAccountID))
            .ReturnsAsync(targetUserAccount);

        accessGuardMock
            .Setup(x => x.EnsureAccessToCompanyMember((int)targetUserAccount.CompanyID!, targetUserAccount.UserLoginDataID))
            .ReturnsAsync(Error.None);

        // Act
        var result = await service.Delete(schedule.ID);

        // Assert
        Assert.True(result.IsSuccess);
        workScheduleRepositoryMock.Verify(x => x.Delete(It.IsAny<WorkSchedule>()), Times.Once);
    }

}
