using MongoDB.Driver;
using teeze.Models;
using BCrypt.Net;

namespace teeze.Services
{
    public class UserServices : IUserServices
    {
        private IMongoCollection<UserModel> _users;

        public UserServices(IOnlineStoreDB OnlineStoreDB, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(OnlineStoreDB.DatabaseName);
            _users = database.GetCollection<UserModel>("Userdata");
        }

        public List<UserModel> GetAllUsers()
        {
            return _users.Find(UserModel => true).ToList();
        }

        public UserModel GetUsersByEmail(string email)
        {
            return _users.Find(UserModel => UserModel.Email == email).FirstOrDefault();
        } 

        public void CreateUser(UserModel newUser)
        {
            var existingUser = GetUsersByEmail(newUser.Email);
            if (existingUser == null)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

                newUser.Password = hashedPassword;

                _users.InsertOne(newUser);
            }
            else
            {
                throw new Exception("Email already exists.");
            }
        }
        public async Task AddSearchHistory(string email, string searchText)
        {
            var filter = Builders<UserModel>.Filter.Eq(user => user.Email, email);
            var update = Builders<UserModel>.Update.Push(user => user.SearchHistory, searchText);
            await _users.UpdateOneAsync(filter, update);
        }
        public async Task<bool> UpdateUserImageAsync(UserModel user)
        {
            var updateDefinition = Builders<UserModel>.Update.Set(u => u.Image64, user.Image64);
            var updateResult = await _users.UpdateOneAsync(u => u.Email == user.Email, updateDefinition);

            return updateResult.ModifiedCount > 0;
        }
        public bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
        }
        public async Task<List<string>> GetSearchHistory(string email)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Email, email);
            var user = await _users.Find(filter).FirstOrDefaultAsync();

            return user?.SearchHistory ?? new List<string>();
        }

    }
}
