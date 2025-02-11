using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OnboardingAPI.Database;
using OnboardingAPI.Models;
using OnboardingAPI.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingAPI.Tests.UnitTests.Users
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserRepositoryImpl _repository;
        private readonly Mock<ILogger<UserRepositoryImpl>> _loggerMock;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDatabase")
                .Options;

            _dbContext = new DatabaseContext(options);
            _loggerMock = new Mock<ILogger<UserRepositoryImpl>>();
            _repository = new UserRepositoryImpl(_dbContext, _loggerMock.Object);
        }

        // Cleanup method to clear the database after each test
        public void Dispose()
        {
            // Clear all users after each test to ensure database isolation
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.SaveChanges();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task AddUserAsync_Should_Add_User_To_Database()
        {
            var user = new User { Id = 1, Name = "John Doe", Email = "john@example.com" };

            var result = await _repository.AddUserAsync(user);
            var retrieved = await _dbContext.Users.FindAsync(1);

            result.Should().BeTrue();
            retrieved.Should().NotBeNull();
            retrieved.Name.Should().Be("John Doe");
        }

        [Fact]
        public async Task GetUserByIdAsync_Should_Return_User_When_Exists()
        {
            var user = new User { Id = 2, Name = "Jane Doe", Email = "jane@example.com" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetUserByIdAsync(2);

            result.Should().NotBeNull();
            result.Name.Should().Be("Jane Doe");
        }

        [Fact]
        public async Task GetUserByNameAsync_Should_Return_User_When_Exists()
        {
            var user = new User { Id = 3, Name = "Alice", Email = "alice@example.com" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetUserByNameAsync("Alice");

            result.Should().NotBeNull();
            result.Email.Should().Be("alice@example.com");
        }

        [Fact]
        public async Task DeleteUserAsync_Should_Remove_User()
        {
            var user = new User { Id = 4, Name = "Bob", Email = "bob@example.com" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.DeleteUserAsync(4);
            var deletedUser = await _dbContext.Users.FindAsync(4);

            result.Should().BeTrue();
            deletedUser.Should().BeNull();
        }

        [Fact]
        public async Task GetAllUsersAsync_Should_Return_List_Of_Users()
        {
            var users = new List<User>
        {
            new User { Id = 5, Name = "Charlie", Email = "charlie@example.com" },
            new User { Id = 6, Name = "Diana", Email = "diana@example.com" }
        };
            await _dbContext.Users.AddRangeAsync(users);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetAllUsersAsync();

            result.Should().HaveCount(2);
        }
    }
}
