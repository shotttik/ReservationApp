using Application.Common.Requests.WorkSchedule;
using Application.Common.Results;
using Application.Interfaces;
using Application.Services;
using Domain.Entities.Common;
using Domain.Entities.User;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Moq;

namespace Application.Tests.Services
{
    public class WorkScheduleExceptionServiceTests
    {
        private readonly Mock<IWorkScheduleRepository> workScheduleRepositoryMock = new();
        private readonly Mock<IAuthService> authServiceMock = new();
        private readonly Mock<IUserAccountRepository> userAccountRepositoryMock = new();
        private readonly Mock<IAccessGuard> accessGuardMock = new();
        private readonly Mock<IWorkScheduleExceptionRepository> workScheduleExceptionMock = new();

        private readonly WorkScheduleService service;

        public WorkScheduleExceptionServiceTests()
        {
            service = new WorkScheduleService(
                workScheduleRepositoryMock.Object,
                userAccountRepositoryMock.Object,
                accessGuardMock.Object,
                authServiceMock.Object,
                workScheduleExceptionMock.Object
            );
        }
        #region Common builders

        private UserAccount BuildUserAccount(int userId, int roleId, int? companyId = 1)
        {
            return new UserAccount
            {
                Id = userId,
                CompanyID = companyId,
                RoleID = roleId,
                FirstName = "Test",
                LastName = "User",
                WorkScheduleExceptions = new List<WorkScheduleException>()
            };
        }

        private WorkScheduleExceptionCreateRequest BuildCreateRequest(int userId, DateOnly start, DateOnly end)
        {
            return new WorkScheduleExceptionCreateRequest
            {
                UserId = userId,
                StartDate = start,
                EndDate = end,
                Type = WorkScheduleExceptionType.Vacation,
                Notes = "Test note"
            };
        }

        private WorkScheduleExceptionUpdateRequest BuildUpdateRequest(int id, int userId, DateOnly start, DateOnly end)
        {
            return new WorkScheduleExceptionUpdateRequest
            {
                Id = id,
                UserId = userId,
                StartDate = start,
                EndDate = end,
                Type = WorkScheduleExceptionType.Vacation,
                Notes = "Updated note"
            };
        }

        private WorkScheduleException BuildException(int id, int userId, DateOnly start, DateOnly end)
        {
            return new WorkScheduleException
            {
                Id = id,
                UserAccountID = userId,
                StartDate = start,
                EndDate = end,
                Type = WorkScheduleExceptionType.Vacation
            };
        }

        #endregion

        #region Create



        [Fact]
        public async Task Create_ShouldSucceed_WhenSuperUserCreatesForCompanyAdmin()
        {
            // Arrange
            var request = BuildCreateRequest(2, new DateOnly(2025, 07, 10), new DateOnly(2025, 07, 12));
            var userAccount = BuildUserAccount(2, Domain.Entities.User.Role.CompanyAdmin.ID);

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(request.UserId))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Error.None);

            workScheduleExceptionMock.Setup(x => x.Add(It.IsAny<WorkScheduleException>()))
                .ReturnsAsync(new WorkScheduleException());

            // Act
            var result = await service.CreateException(request);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Create_ShouldSucceed_WhenCompanyAdminCreatesForSelf()
        {
            var userId = 2;
            var request = BuildCreateRequest(userId, new DateOnly(2025, 07, 10), new DateOnly(2025, 07, 10));
            var userAccount = BuildUserAccount(userId, Domain.Entities.User.Role.CompanyAdmin.ID);

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(userId))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Error.None);

            workScheduleExceptionMock.Setup(x => x.Add(It.IsAny<WorkScheduleException>()))
                .ReturnsAsync(new WorkScheduleException());

            var result = await service.CreateException(request);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Create_ShouldSucceed_WhenCompanyAdminCreatesForCompanyEmployeeInSameCompany()
        {
            var request = BuildCreateRequest(3, new DateOnly(2025, 07, 11), new DateOnly(2025, 07, 11));
            var userAccount = BuildUserAccount(3, Domain.Entities.User.Role.CompanyEmployee.ID);

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(request.UserId))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Error.None);

            workScheduleExceptionMock.Setup(x => x.Add(It.IsAny<WorkScheduleException>()))
                .ReturnsAsync(new WorkScheduleException());

            var result = await service.CreateException(request);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Create_ShouldSucceed_WhenCompanyEmployeeCreatesForSelf()
        {
            var userId = 4;
            var request = BuildCreateRequest(userId, new DateOnly(2025, 07, 12), new DateOnly(2025, 07, 12));
            var userAccount = BuildUserAccount(userId, Domain.Entities.User.Role.CompanyEmployee.ID);

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(userId))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Error.None);

            workScheduleExceptionMock.Setup(x => x.Add(It.IsAny<WorkScheduleException>()))
                .ReturnsAsync(new WorkScheduleException());

            var result = await service.CreateException(request);

            Assert.True(result.IsSuccess);
        }



        [Fact]
        public async Task Create_ShouldFail_WhenCompanyAdminCreatesForEmployeeInDifferentCompany()
        {
            var request = BuildCreateRequest(5, new DateOnly(2025, 07, 13), new DateOnly(2025, 07, 13));
            var userAccount = BuildUserAccount(5, Domain.Entities.User.Role.CompanyEmployee.ID, 2);

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(request.UserId))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(GenericResults.Forbidden);

            var result = await service.CreateException(request);

            Assert.True(result.IsFailure);
            Assert.Equal(GenericResults.Forbidden, result.Error);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenCompanyEmployeeCreatesForAnotherEmployee()
        {
            var request = BuildCreateRequest(6, new DateOnly(2025, 07, 14), new DateOnly(2025, 07, 14));
            var userAccount = BuildUserAccount(6, Domain.Entities.User.Role.CompanyEmployee.ID);

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(request.UserId))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(GenericResults.Forbidden);

            var result = await service.CreateException(request);

            Assert.True(result.IsFailure);
            Assert.Equal(GenericResults.Forbidden, result.Error);
        }



        [Fact]
        public async Task Create_ShouldFail_WhenStartDateAfterEndDate()
        {
            var request = BuildCreateRequest(1, new DateOnly(2025, 07, 12), new DateOnly(2025, 07, 10));

            var result = await service.CreateException(request);

            Assert.True(result.IsFailure);
            Assert.Equal(WorkScheduleResults.InvalidDateRange, result.Error);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenUserAccountNotFound()
        {
            var request = BuildCreateRequest(7, new DateOnly(2025, 07, 10), new DateOnly(2025, 07, 12));

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(request.UserId))
                .ReturnsAsync((UserAccount)null);

            var result = await service.CreateException(request);

            Assert.True(result.IsFailure);
            Assert.Equal(GenericResults.DontExists, result.Error);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenOverlappingExceptionExists()
        {
            var userId = 8;
            var request = BuildCreateRequest(userId, new DateOnly(2025, 07, 10), new DateOnly(2025, 07, 12));
            var existing = new WorkScheduleException
            {
                Id = 1,
                UserAccountID = userId,
                StartDate = new DateOnly(2025, 07, 11),
                EndDate = new DateOnly(2025, 07, 13),
                Type = WorkScheduleExceptionType.Vacation
            };

            var userAccount = BuildUserAccount(userId, Domain.Entities.User.Role.CompanyEmployee.ID);
            userAccount.WorkScheduleExceptions.Add(existing);

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(userId))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Error.None);

            var result = await service.CreateException(request);

            Assert.True(result.IsFailure);
            Assert.Equal(WorkScheduleResults.OverlappingException, result.Error);
        }

        #endregion
        #region Update

        [Fact]
        public async Task Update_ShouldSucceed_WhenValidRequest()
        {
            var userId = 1;
            var request = BuildUpdateRequest(10, userId, new DateOnly(2025, 07, 10), new DateOnly(2025, 07, 12));
            var exception = BuildException(10, userId, new DateOnly(2025, 07, 09), new DateOnly(2025, 07, 11));
            var userAccount = BuildUserAccount(userId, Domain.Entities.User.Role.CompanyEmployee.ID);
            userAccount.WorkScheduleExceptions.Add(exception);

            workScheduleExceptionMock.Setup(x => x.Get(request.Id))
                .ReturnsAsync(exception);

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(userId))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Error.None);

            workScheduleExceptionMock.Setup(x => x.Update(It.IsAny<WorkScheduleException>()))
                .Returns(Task.CompletedTask);

            var result = await service.UpdateException(request);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenStartDateAfterEndDate()
        {
            var request = BuildUpdateRequest(10, 1, new DateOnly(2025, 07, 12), new DateOnly(2025, 07, 10));

            var result = await service.UpdateException(request);

            Assert.True(result.IsFailure);
            Assert.Equal(WorkScheduleResults.InvalidDateRange, result.Error);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenExceptionNotFound()
        {
            var request = BuildUpdateRequest(10, 1, new DateOnly(2025, 07, 10), new DateOnly(2025, 07, 12));

            workScheduleExceptionMock.Setup(x => x.Get(request.Id))
                .ReturnsAsync((WorkScheduleException)null);

            var result = await service.UpdateException(request);

            Assert.True(result.IsFailure);
            Assert.Equal(WorkScheduleResults.DoesntExists, result.Error);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenUserAccountNotFound()
        {
            var request = BuildUpdateRequest(10, 1, new DateOnly(2025, 07, 10), new DateOnly(2025, 07, 12));
            var exception = BuildException(10, 1, new DateOnly(2025, 07, 09), new DateOnly(2025, 07, 11));

            workScheduleExceptionMock.Setup(x => x.Get(request.Id))
                .ReturnsAsync(exception);

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(request.UserId))
                .ReturnsAsync((UserAccount)null);

            var result = await service.UpdateException(request);

            Assert.True(result.IsFailure);
            Assert.Equal(GenericResults.DontExists, result.Error);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenAccessDenied()
        {
            var userId = 1;
            var request = BuildUpdateRequest(10, userId, new DateOnly(2025, 07, 10), new DateOnly(2025, 07, 12));
            var exception = BuildException(10, userId, new DateOnly(2025, 07, 09), new DateOnly(2025, 07, 11));
            var userAccount = BuildUserAccount(userId, Domain.Entities.User.Role.CompanyEmployee.ID);
            userAccount.WorkScheduleExceptions.Add(exception);

            workScheduleExceptionMock.Setup(x => x.Get(request.Id))
                .ReturnsAsync(exception);

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(userId))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(GenericResults.Forbidden);

            var result = await service.UpdateException(request);

            Assert.True(result.IsFailure);
            Assert.Equal(GenericResults.Forbidden, result.Error);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenOverlappingExceptionExists()
        {
            var userId = 1;
            var request = BuildUpdateRequest(10, userId, new DateOnly(2025, 07, 10), new DateOnly(2025, 07, 12));
            var exception = BuildException(10, userId, new DateOnly(2025, 07, 09), new DateOnly(2025, 07, 11));

            var overlapping = BuildException(11, userId, new DateOnly(2025, 07, 11), new DateOnly(2025, 07, 13));

            var userAccount = BuildUserAccount(userId, Domain.Entities.User.Role.CompanyEmployee.ID);
            userAccount.WorkScheduleExceptions.Add(overlapping);

            workScheduleExceptionMock.Setup(x => x.Get(request.Id))
                .ReturnsAsync(exception);

            userAccountRepositoryMock.Setup(x => x.GetByUserLoginDataIDWithWorkScheduleExceptions(userId))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Error.None);

            var result = await service.UpdateException(request);

            Assert.True(result.IsFailure);
            Assert.Equal(WorkScheduleResults.OverlappingException, result.Error);
        }

        #endregion
        #region Delete

        [Fact]
        public async Task Delete_ShouldSucceed_WhenValidRequest()
        {
            var userId = 1;
            var exceptionId = 10;
            var exception = BuildException(exceptionId, userId, new DateOnly(2025, 07, 09), new DateOnly(2025, 07, 11));
            var userAccount = BuildUserAccount(userId, Domain.Entities.User.Role.CompanyEmployee.ID);

            workScheduleExceptionMock.Setup(x => x.Get(exceptionId))
                .ReturnsAsync(exception);

            userAccountRepositoryMock.Setup(x => x.Get(exception.UserAccountID))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Error.None);

            workScheduleExceptionMock.Setup(x => x.Delete(It.IsAny<WorkScheduleException>()))
                .Returns(Task.CompletedTask);

            var result = await service.DeleteException(exceptionId);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Delete_ShouldFail_WhenExceptionNotFound()
        {
            var exceptionId = 10;

            workScheduleExceptionMock.Setup(x => x.Get(exceptionId))
                .ReturnsAsync((WorkScheduleException)null);

            var result = await service.DeleteException(exceptionId);

            Assert.True(result.IsFailure);
            Assert.Equal(WorkScheduleResults.DoesntExists, result.Error);
        }

        [Fact]
        public async Task Delete_ShouldFail_WhenUserAccountNotFound()
        {
            var exceptionId = 10;
            var exception = BuildException(exceptionId, 1, new DateOnly(2025, 07, 09), new DateOnly(2025, 07, 11));

            workScheduleExceptionMock.Setup(x => x.Get(exceptionId))
                .ReturnsAsync(exception);

            userAccountRepositoryMock.Setup(x => x.Get(exception.UserAccountID))
                .ReturnsAsync((UserAccount)null);

            var result = await service.DeleteException(exceptionId);

            Assert.True(result.IsFailure);
            Assert.Equal(GenericResults.DontExists, result.Error);
        }

        [Fact]
        public async Task Delete_ShouldFail_WhenUserAccountCompanyIDNull()
        {
            var exceptionId = 10;
            var exception = BuildException(exceptionId, 1, new DateOnly(2025, 07, 09), new DateOnly(2025, 07, 11));
            var userAccount = BuildUserAccount(1, Domain.Entities.User.Role.CompanyEmployee.ID, null);

            workScheduleExceptionMock.Setup(x => x.Get(exceptionId))
                .ReturnsAsync(exception);

            userAccountRepositoryMock.Setup(x => x.Get(exception.UserAccountID))
                .ReturnsAsync(userAccount);

            var result = await service.DeleteException(exceptionId);

            Assert.True(result.IsFailure);
            Assert.Equal(GenericResults.DontExists, result.Error);
        }

        [Fact]
        public async Task Delete_ShouldFail_WhenAccessDenied()
        {
            var exceptionId = 10;
            var exception = BuildException(exceptionId, 1, new DateOnly(2025, 07, 09), new DateOnly(2025, 07, 11));
            var userAccount = BuildUserAccount(1, Domain.Entities.User.Role.CompanyEmployee.ID);

            workScheduleExceptionMock.Setup(x => x.Get(exceptionId))
                .ReturnsAsync(exception);

            userAccountRepositoryMock.Setup(x => x.Get(exception.UserAccountID))
                .ReturnsAsync(userAccount);

            accessGuardMock.Setup(x => x.EnsureAccessToCompanyEmployee(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(GenericResults.Forbidden);

            var result = await service.DeleteException(exceptionId);

            Assert.True(result.IsFailure);
            Assert.Equal(GenericResults.Forbidden, result.Error);
        }

        #endregion
    }
}
