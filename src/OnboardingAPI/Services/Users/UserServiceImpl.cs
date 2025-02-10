using OnboardingAPI.Models;
using OnboardingAPI.Repositories.Users;

namespace OnboardingAPI.Services.Users
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserServiceImpl(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUser(User user)
        {
            var userCreated = await _userRepository.CreateUser(user);

            return userCreated;
        }

        public async Task<User> GetUser(int id)
        {
            var userInDb = await _userRepository.GetUser(id);

            return userInDb;
        }

        public async Task<User> UpdateUser(User user)
        {
            var userUpdated = await _userRepository.UpdateUser(user);

            return userUpdated;
        }
        public Task<int> DeleteUser(int id)
        {
            return _userRepository.DeleteUser(id);
        }
    }
}
