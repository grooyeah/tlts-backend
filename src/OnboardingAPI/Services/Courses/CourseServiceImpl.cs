using FluentValidation;
using OnboardingAPI.Models;
using OnboardingAPI.Repositories.Courses;

namespace OnboardingAPI.Services.Courses
{
    public class CourseServiceImpl : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IValidator<Course> _validator;

        public CourseServiceImpl(ICourseRepository courseRepository, IValidator<Course> validator)
        {
            _courseRepository = courseRepository;
            _validator = validator;
        }

        public async Task<ServiceResult<IEnumerable<Course>>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return ServiceResult<IEnumerable<Course>>.CreateSuccess(courses);
        }

        public async Task<ServiceResult<Course>> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            return course != null
                ? ServiceResult<Course>.CreateSuccess(course)
                : ServiceResult<Course>.CreateFailure("Course not found");
        }

        public async Task<ServiceResult<Course>> CreateCourseAsync(Course course)
        {
            var validationResult = await _validator.ValidateAsync(course);
            if (!validationResult.IsValid)
            {
                return ServiceResult<Course>.CreateFailure(string.Join("; ", validationResult.Errors));
            }

            await _courseRepository.AddAsync(course);
            return ServiceResult<Course>.CreateSuccess(course);
        }

        public async Task<ServiceResult<bool>> UpdateCourseAsync(Course course)
        {
            var validationResult = await _validator.ValidateAsync(course);
            if (!validationResult.IsValid)
            {
                return ServiceResult<bool>.CreateFailure(string.Join("; ", validationResult.Errors));
            }

            await _courseRepository.UpdateAsync(course);
            return ServiceResult<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResult<bool>> DeleteCourseAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return ServiceResult<bool>.CreateFailure("Course not found");
            }

            await _courseRepository.DeleteAsync(id);
            return ServiceResult<bool>.CreateSuccess(true);
        }
    }
}
