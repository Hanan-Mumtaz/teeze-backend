using teeze.Models;
using System.Collections.Generic;

namespace teeze.Services
{
    public interface IUserServices
    {
        List<UserModel> GetAllUsers(); 
        UserModel GetUsersByEmail(string email);
        void CreateUser(UserModel newUser);
        public Task AddSearchHistory(string email, string searchText);
        public Task<bool> UpdateUserImageAsync(UserModel user);
        public Task<List<string>> GetSearchHistory(string email);

    }
}