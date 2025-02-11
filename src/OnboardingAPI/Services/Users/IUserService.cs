using OnboardingAPI.Models;

namespace OnboardingAPI.Services.Users
{
    public interface IUserService
    {
        Task<ServiceResult<User>> CreateUserAsync(User user);
        Task<ServiceResult<User>> GetUserByIdAsync(int id);
        Task<ServiceResult<User>> UpdateUserAsync(int id, User user);
        Task<ServiceResult<bool>> DeleteUserAsync(int id);
        Task<ServiceResult<User>> GetUserByNameAsync(string name);
        Task<ServiceResult<List<User>>> GetAllUsersAsync();
    }
}
