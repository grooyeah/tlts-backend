using FluentValidation;
using OnboardingAPI.Models;
using OnboardingAPI.Repositories.Users;

namespace OnboardingAPI.Services.Users
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _userValidator;

        public UserServiceImpl(IUserRepository userRepository, IValidator<User> userValidator)
        {
            _userRepository = userRepository;
            _userValidator = userValidator;
        }

        public async Task<ServiceResult<User>> CreateUserAsync(User user)
        {
            var validationResult = await _userValidator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                return ServiceResult<User>.CreateFailure("Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var userExists = await _userRepository.GetUserByNameAsync(user.Name);

            if (userExists != null)
            {
                return ServiceResult<User>.CreateFailure("User already exists.");
            }

            var success = await _userRepository.AddUserAsync(user);

            return success ? ServiceResult<User>.CreateSuccess(user) : ServiceResult<User>.CreateFailure("Failed to create user.");
        }

        public async Task<ServiceResult<User>> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            return user != null ? ServiceResult<User>.CreateSuccess(user) : ServiceResult<User>.CreateFailure("User not found.");
        }

        public async Task<ServiceResult<User>> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);

            if (existingUser == null)
            {
                return ServiceResult<User>.CreateFailure("User not found.");
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;


            var success = await _userRepository.UpdateUserAsync(existingUser);

            return success ? ServiceResult<User>.CreateSuccess(existingUser) : ServiceResult<User>.CreateFailure("Failed to update user.");
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return ServiceResult<bool>.CreateFailure("User not found.");
            }

            var success = await _userRepository.DeleteUserAsync(id);

            return success ? ServiceResult<bool>.CreateSuccess(true) : ServiceResult<bool>.CreateFailure("Failed to delete user.");
        }

        public async Task<ServiceResult<User>> GetUserByNameAsync(string name)
        {
            var user = await _userRepository.GetUserByNameAsync(name);

            return user != null ? ServiceResult<User>.CreateSuccess(user) : ServiceResult<User>.CreateFailure("User not found.");
        }

        public async Task<ServiceResult<List<User>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users.Any() ? ServiceResult<List<User>>.CreateSuccess(users) : ServiceResult<List<User>>.CreateFailure("No users found.");
        }
    }
}
