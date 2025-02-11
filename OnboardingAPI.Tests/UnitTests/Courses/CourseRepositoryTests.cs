using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OnboardingAPI.Database;
using OnboardingAPI.Models;
using OnboardingAPI.Repositories.Courses;

namespace OnboardingAPI.Tests.UnitTests.Courses
{
    public class CourseRepositoryTests
    {
        private readonly DatabaseContext _dbContext;
        private readonly CourseRepositoryImpl _repository;

        public CourseRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new DatabaseContext(options);
            _repository = new CourseRepositoryImpl(_dbContext);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Course_To_Database()
        {
            var course = new Course { Id = 1, Title = "Test Course" };

            await _repository.AddAsync(course);
            var retrieved = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == 1);

            retrieved.Should().NotBeNull();
            retrieved.Title.Should().Be("Test Course");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Course_When_Exists()
        {
            var course = new Course { Id = 2, Title = "Existing Course" };
            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(2);

            result.Should().NotBeNull();
            result.Title.Should().Be("Existing Course");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Course()
        {
            var course = new Course { Id = 3, Title = "To be deleted" };
            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();

            await _repository.DeleteAsync(3);
            var result = await _dbContext.Courses.FindAsync(3);

            result.Should().BeNull();
        }
    }
}
