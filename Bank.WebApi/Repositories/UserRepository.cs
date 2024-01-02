using Bank.WebApi.Interfaces;
using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;
using Dapper;
using System.Data;

namespace Bank.WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<UserEntity> GetUserAsync(int id)
        {
            var userQuery = "SELECT * FROM Users WHERE UserID = @Id";
            var user = await _db.QueryFirstOrDefaultAsync<UserEntity>(userQuery, new { Id = id });

            if (user != null)
            {
                var accountsQuery = "SELECT * FROM Accounts WHERE UserId = @UserId";
                var accounts = await _db.QueryAsync<AccountEntity>(accountsQuery, new { UserId = id });
                user.Accounts = accounts.Select(a => new Account
                {
                    UserId = a.UserId,
                    Type = a.Type,
                    Balance = a.Balance
                }).ToList();
            }

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var query = "SELECT * FROM Users";
            return await _db.QueryAsync<User>(query);
        }

        public async Task CreateUserAsync(UserEntity user)
        {
            var query = "INSERT INTO Users (Name, Address) VALUES (@Name, @Address)";
            await _db.ExecuteAsync(query, user);
        }

        public async Task UpdateUserAsync(User user)
        {
            var query = "UPDATE Users SET Name = @Name, Address = @Address WHERE UserID = @UserId";
            await _db.ExecuteAsync(query, user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var query = "DELETE FROM Users WHERE UserID = @Id";
            await _db.ExecuteAsync(query, new { Id = id });
        }
    }
}
