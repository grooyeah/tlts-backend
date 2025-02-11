using Microsoft.EntityFrameworkCore;
using OnboardingAPI.Database;
using OnboardingAPI.Models;

namespace OnboardingAPI.Repositories.Users
{
    public class UserRepositoryImpl : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<UserRepositoryImpl> _logger;

        public UserRepositoryImpl(DatabaseContext context, ILogger<UserRepositoryImpl> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
    }
}
