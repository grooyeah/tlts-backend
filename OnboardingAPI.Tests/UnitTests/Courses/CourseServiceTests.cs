using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using OnboardingAPI.Models;
using OnboardingAPI.Repositories.Courses;
using OnboardingAPI.Services.Courses;

namespace OnboardingAPI.Tests.UnitTests.Courses
{
    public class CourseServiceTests
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<IValidator<Course>> _validatorMock;
        private readonly ICourseService _courseService;

        public CourseServiceTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _validatorMock = new Mock<IValidator<Course>>();
            _courseService = new CourseServiceImpl(_courseRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task GetAllCoursesAsync_Should_Return_Courses()
        {
            var courses = new List<Course>
            {
                new Course { Id = 1, Title = "Course 1" },
                new Course { Id = 2, Title = "Course 2" }
            };
            _courseRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(courses);

            var result = await _courseService.GetAllCoursesAsync();

            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(2);
        }

        [Fact]
        public async Task CreateCourseAsync_Should_Validate_And_Create_Course()
        {
            var course = new Course { Id = 3, Title = "New Course" };
            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<Course>(), default))
                .ReturnsAsync(new ValidationResult());

            _courseRepositoryMock.Setup(repo => repo.AddAsync(course)).Returns(Task.CompletedTask);

            var result = await _courseService.CreateCourseAsync(course);

            result.Success.Should().BeTrue();
            _courseRepositoryMock.Verify(repo => repo.AddAsync(course), Times.Once);
        }

        [Fact]
        public async Task CreateCourseAsync_Should_Fail_When_Validation_Fails()
        {
            var course = new Course { Id = 4, Title = "" };
            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<Course>(), default))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Name", "Name is required")
                }));

            var result = await _courseService.CreateCourseAsync(course);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Name is required");
        }
    }
}
