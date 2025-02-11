using Microsoft.AspNetCore.Mvc;
using OnboardingAPI.Models;
using OnboardingAPI.Services.Courses;

namespace OnboardingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            var result = await _courseService.GetAllCoursesAsync();
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseById(int id)
        {
            var result = await _courseService.GetCourseByIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.Message);
        }

        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(Course course)
        {
            var result = await _courseService.CreateCourseAsync(course);
            return result.Success
                ? CreatedAtAction(nameof(GetCourseById), new { id = result.Data!.Id }, result.Data)
                : BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var result = await _courseService.UpdateCourseAsync(course);
            return result.Success ? NoContent() : BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);
            return result.Success ? NoContent() : NotFound(result.Message);
        }
    }
}
