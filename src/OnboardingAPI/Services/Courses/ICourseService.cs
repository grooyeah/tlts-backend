using OnboardingAPI.Models;

namespace OnboardingAPI.Services.Courses
{
    public interface ICourseService
    {
        Task<ServiceResult<IEnumerable<Course>>> GetAllCoursesAsync();
        Task<ServiceResult<Course>> GetCourseByIdAsync(int id);
        Task<ServiceResult<Course>> CreateCourseAsync(Course course);
        Task<ServiceResult<bool>> UpdateCourseAsync(Course course);
        Task<ServiceResult<bool>> DeleteCourseAsync(int id);
    }
}
