using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnboardingAPI.Database;
using OnboardingAPI.Models;
using OnboardingAPI.Services.Users;

namespace OnboardingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var result = await _userService.CreateUserAsync(user);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetUserById), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var result = await _userService.UpdateUserAsync(id, user);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return NoContent(); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data); 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data); 
        }
    }
}
