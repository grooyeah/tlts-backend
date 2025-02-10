using OnboardingAPI.Models;

namespace OnboardingAPI.Services.Users
{
    public interface IUserService
    {
        //Crud 

        //Create
        // Return something else (ok or something)
        public Task<User> CreateUser(User user);
        //Read
        public Task<User> GetUser(int  id);
        //Update
        public Task<User> UpdateUser(User user);
        //Delete
        public Task<int> DeleteUser(int id);
    }
}
