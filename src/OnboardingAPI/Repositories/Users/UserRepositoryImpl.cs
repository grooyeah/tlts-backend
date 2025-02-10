using Microsoft.EntityFrameworkCore;
using OnboardingAPI.Database;
using OnboardingAPI.Models;
using System.Data.Common;

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

        public async Task<User> CreateUser(User user)
        {
            var existingUser = _context.Users.AnyAsync(x => x.Name ==  user.Name).Result;

            if(existingUser)
            {
                _logger.LogWarning("User already exists");
                return null;
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public Task<User> GetUser(int id)
        {
            var existingUser = _context.Users.AnyAsync(x => x.Id == id).Result;

            if(!existingUser)
            {
                _logger.LogWarning("User doesn't exist");
                return null;
            }

            var userInDb = _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            return userInDb;
        }

        public async Task<User> UpdateUser(User user)
        {
            var existingUser = _context.Users.AnyAsync(x => x.Id == user.Id).Result;

            if(!existingUser)
            {
                _logger.LogWarning("User could not be found");
                return null;
            }

            var userInDb = await _context.Users.SingleOrDefaultAsync(x => x.Id == user.Id);
            
            userInDb.Name = user.Name;
            userInDb.Email = user.Email;
            userInDb.PasswordHash = user.PasswordHash;
            userInDb.Role = user.Role;

            await _context.SaveChangesAsync();

            return userInDb;
        }
        public async Task<int> DeleteUser(int id)
        {
            var existingUser = _context.Users.AnyAsync(x => x.Id == id).Result;

            if(! existingUser)
            {
                _logger.LogWarning("User could not be found");
                return id;
            }

            var userInDb = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            _context.Remove<User>(userInDb);
            await _context.SaveChangesAsync();

            return id;
        }
    }
}
