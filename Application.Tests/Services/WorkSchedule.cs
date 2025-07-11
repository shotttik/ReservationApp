using Application.Common.Requests.WorkSchedule;
using Application.Common.Results;
using Application.Interfaces;
using Application.Services;
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
    private readonly Mock<IWorkScheduleExceptionRepository> workScheduleExceptionMock = new();

    private readonly WorkScheduleService service;

    public WorkScheduleServiceTests()
    {
        service = new WorkScheduleService(
            workScheduleRepositoryMock.Object,
            userAccountRepositoryMock.Object,
            accessGuardMock.Object,
            authServiceMock.Object,
            workScheduleExceptionMock.Object
        );
    }

    #region Builders

    private UserAccount BuildUserAccount(int id, int roleId, int? companyId = 1)
    {
        return new UserAccount
        {
            ID = id,
            CompanyID = companyId,
            RoleID = roleId,
            FirstName = "Test",
            LastName = "User",
            WorkSchedules = new List<WorkSchedule>()
        };
    }

    private WorkScheduleCreateRequest BuildCreateRequest(int userId, TimeOnly start, TimeOnly end)
    {
        return new WorkScheduleCreateRequest
        {
            UserID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = start,
            EndTime = end
        };
    }

    private WorkSchedule BuildWorkSchedule(int id, int userId, TimeOnly start, TimeOnly end)
    {
        return new WorkSchedule
        {
            ID = id,
            UserAccountID = userId,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = start,
            EndTime = end
        };
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_ShouldReturnSuccess_WhenValidRequest()
    {
        // Arrange
        var request = BuildCreateRequest(1, new TimeOnly(9, 0), new TimeOnly(12, 0));
        var userAccount = BuildUserAccount(1, Role.CompanyMember.ID);

        userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(1))
            .ReturnsAsync(userAccount);

        accessGuardMock.Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Error.None);

        workScheduleRepositoryMock.Setup(x => x.Add(It.IsAny<WorkSchedule>()))
            .ReturnsAsync(new WorkSchedule());

        // Act
        var result = await service.Create(request);

        // Assert
        Assert.True(result.IsSuccess);
        workScheduleRepositoryMock.Verify(x => x.Add(It.IsAny<WorkSchedule>()), Times.Once);
        workScheduleRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Create_ShouldFail_WhenStartTimeAfterEndTime()
    {
        var request = BuildCreateRequest(1, new TimeOnly(14, 0), new TimeOnly(12, 0));

        var result = await service.Create(request);

        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.InvalidTimeRange, result.Error);
        workScheduleRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Create_ShouldFail_WhenOverlappingScheduleExists()
    {
        var userId = 1;
        var request = BuildCreateRequest(userId, new TimeOnly(10, 0), new TimeOnly(13, 0));

        var existingSchedule = BuildWorkSchedule(2, userId, new TimeOnly(9, 0), new TimeOnly(11, 0));
        var userAccount = BuildUserAccount(userId, Role.CompanyMember.ID);
        userAccount.WorkSchedules.Add(existingSchedule);

        userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync(userAccount);

        accessGuardMock.Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Error.None);

        var result = await service.Create(request);

        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.OverlappingSchedule, result.Error);
        workScheduleRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Create_ShouldFail_WhenAccessDenied()
    {
        var userId = 1;
        var request = BuildCreateRequest(userId, new TimeOnly(9, 0), new TimeOnly(12, 0));
        var userAccount = BuildUserAccount(userId, Role.CompanyMember.ID);

        userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync(userAccount);

        accessGuardMock.Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(GenericResults.Forbidden);

        var result = await service.Create(request);

        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
        workScheduleRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Create_ShouldFail_WhenUserAccountNotFound()
    {
        var userId = 1;
        var request = BuildCreateRequest(userId, new TimeOnly(9, 0), new TimeOnly(12, 0));

        userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId))
            .ReturnsAsync((UserAccount)null);

        var result = await service.Create(request);

        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.DontExists, result.Error);
        workScheduleRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Create_ShouldFail_WhenStartTimeEqualsEndTime()
    {
        var request = BuildCreateRequest(1, new TimeOnly(12, 0), new TimeOnly(12, 0));

        var result = await service.Create(request);

        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.InvalidTimeRange, result.Error);
        workScheduleRepositoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_ShouldSucceed_WhenValidRequest()
    {
        // Arrange
        var userId = 1;
        var schedule = BuildWorkSchedule(10, userId, new TimeOnly(9, 0), new TimeOnly(11, 0));
        var userAccount = BuildUserAccount(userId, Role.CompanyMember.ID);
        userAccount.WorkSchedules.Add(schedule);

        workScheduleRepositoryMock.Setup(x => x.Get(schedule.ID)).ReturnsAsync(schedule);
        userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId)).ReturnsAsync(userAccount);
        accessGuardMock.Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), userId)).ReturnsAsync(Error.None);
        workScheduleRepositoryMock.Setup(x => x.Update(schedule)).Returns(Task.CompletedTask);

        var request = new WorkScheduleUpdateRequest
        {
            ID = schedule.ID,
            UserID = userId,
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 0)
        };

        // Act
        var result = await service.Update(request);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Update_ShouldFail_WhenScheduleNotFound()
    {
        workScheduleRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((WorkSchedule)null);

        var request = new WorkScheduleUpdateRequest { ID = 1, UserID = 1, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(12, 0) };
        var result = await service.Update(request);

        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.DoesntExists, result.Error);
    }

    [Fact]
    public async Task Update_ShouldFail_WhenStartTimeAfterEndTime()
    {
        var schedule = BuildWorkSchedule(10, 1, new TimeOnly(9, 0), new TimeOnly(11, 0));
        workScheduleRepositoryMock.Setup(x => x.Get(schedule.ID)).ReturnsAsync(schedule);

        var request = new WorkScheduleUpdateRequest { ID = 10, UserID = 1, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(12, 0) };
        var result = await service.Update(request);

        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.InvalidTimeRange, result.Error);
    }

    [Fact]
    public async Task Update_ShouldFail_WhenOverlappingWithOtherSchedule()
    {
        var userId = 1;
        var schedule = BuildWorkSchedule(10, userId, new TimeOnly(9, 0), new TimeOnly(11, 0));
        var overlapping = BuildWorkSchedule(11, userId, new TimeOnly(10, 0), new TimeOnly(12, 0));

        var userAccount = BuildUserAccount(userId, Role.CompanyMember.ID);
        userAccount.WorkSchedules.Add(schedule); // ✅ Add the schedule being updated
        userAccount.WorkSchedules.Add(overlapping); // ✅ Add the overlapping one

        workScheduleRepositoryMock.Setup(x => x.Get(schedule.ID)).ReturnsAsync(schedule);
        userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkSchedules(userId)).ReturnsAsync(userAccount);
        accessGuardMock.Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), userId)).ReturnsAsync(Error.None);

        var request = new WorkScheduleUpdateRequest
        {
            ID = schedule.ID,
            UserID = userId,
            StartTime = new TimeOnly(10, 30),
            EndTime = new TimeOnly(12, 30)
        };

        var result = await service.Update(request);

        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.OverlappingSchedule, result.Error);
    }


    #endregion

    #region Delete

    [Fact]
    public async Task Delete_ShouldSucceed_WhenValid()
    {
        var userId = 1;
        var schedule = BuildWorkSchedule(10, userId, new TimeOnly(9, 0), new TimeOnly(12, 0));
        var userAccount = BuildUserAccount(userId, Role.CompanyMember.ID);

        workScheduleRepositoryMock.Setup(x => x.Get(schedule.ID)).ReturnsAsync(schedule);
        userAccountRepositoryMock.Setup(x => x.Get(schedule.UserAccountID)).ReturnsAsync(userAccount);
        accessGuardMock.Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(Error.None);
        workScheduleRepositoryMock.Setup(x => x.Delete(schedule)).Returns(Task.CompletedTask);

        var result = await service.Delete(schedule.ID);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_ShouldFail_WhenScheduleNotFound()
    {
        workScheduleRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((WorkSchedule)null);

        var result = await service.Delete(10);

        Assert.True(result.IsFailure);
        Assert.Equal(WorkScheduleResults.DoesntExists, result.Error);
    }

    [Fact]
    public async Task Delete_ShouldFail_WhenAccessDenied()
    {
        var schedule = BuildWorkSchedule(10, 1, new TimeOnly(9, 0), new TimeOnly(12, 0));
        var userAccount = BuildUserAccount(1, Role.CompanyMember.ID);

        workScheduleRepositoryMock.Setup(x => x.Get(schedule.ID)).ReturnsAsync(schedule);
        userAccountRepositoryMock.Setup(x => x.Get(schedule.UserAccountID)).ReturnsAsync(userAccount);
        accessGuardMock.Setup(x => x.EnsureAccessToCompanyMember(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(GenericResults.Forbidden);

        var result = await service.Delete(schedule.ID);

        Assert.True(result.IsFailure);
        Assert.Equal(GenericResults.Forbidden, result.Error);
    }

    #endregion
}

