using Dapper;
using LibraryManagementSystem.Models;
using System.Data;

namespace LibraryManagementSystem.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            string sql = "SELECT * FROM Users WHERE Username = @Username";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task CreateUserAsync(User user)
        {
            string sql = @"INSERT INTO Users (Username, PasswordHash, Email, Role)
                       VALUES (@Username, @PasswordHash, @Email, @Role)";
            await _db.ExecuteAsync(sql, user);
        }
    }
}
