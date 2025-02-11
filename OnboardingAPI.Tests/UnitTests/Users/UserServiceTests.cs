using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using OnboardingAPI.Models;
using OnboardingAPI.Repositories.Users;
using OnboardingAPI.Services.Users;

namespace OnboardingAPI.Tests.UnitTests.Users
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IValidator<User>> _validatorMock;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<User>>();
            _userService = new UserServiceImpl(_userRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_Should_Validate_And_Create_User()
        {
            var user = new User { Id = 1, Name = "John Doe", Email = "john@example.com" };

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<User>(), default))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock
                .Setup(repo => repo.GetUserByNameAsync(user.Name))
                .ReturnsAsync((User)null);

            _userRepositoryMock
                .Setup(repo => repo.AddUserAsync(user))
                .ReturnsAsync(true);

            var result = await _userService.CreateUserAsync(user);

            result.Success.Should().BeTrue();
            _userRepositoryMock.Verify(repo => repo.AddUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_Should_Fail_When_Validation_Fails()
        {
            var user = new User { Id = 2, Name = "", Email = "invalid@example.com" };

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<User>(), default))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Name", "Name is required")
                }));

            var result = await _userService.CreateUserAsync(user);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Name is required");
        }

        [Fact]
        public async Task CreateUserAsync_Should_Fail_When_User_Already_Exists()
        {
            var user = new User { Id = 3, Name = "Existing User", Email = "existing@example.com" };

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<User>(), default))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock
                .Setup(repo => repo.GetUserByNameAsync(user.Name))
                .ReturnsAsync(user);

            var result = await _userService.CreateUserAsync(user);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("User already exists");
        }

        [Fact]
        public async Task GetUserByIdAsync_Should_Return_User_When_Found()
        {
            var user = new User { Id = 4, Name = "Jane Doe", Email = "jane@example.com" };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(4))
                .ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(4);

            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GetUserByIdAsync_Should_Return_Failure_When_User_Not_Found()
        {
            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(5))
                .ReturnsAsync((User)null);

            var result = await _userService.GetUserByIdAsync(5);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("User not found");
        }

        [Fact]
        public async Task DeleteUserAsync_Should_Return_Success_When_User_Deleted()
        {
            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(6))
                .ReturnsAsync(new User { Id = 6, Name = "DeleteMe" });

            _userRepositoryMock
                .Setup(repo => repo.DeleteUserAsync(6))
                .ReturnsAsync(true);

            var result = await _userService.DeleteUserAsync(6);

            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_Should_Return_Failure_When_User_Not_Found()
        {
            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(7))
                .ReturnsAsync((User)null);

            var result = await _userService.DeleteUserAsync(7);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("User not found");
        }
    }
}
