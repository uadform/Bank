using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;

namespace Bank.WebApi.Interfaces
{
    public interface IUserService
    {
        public Task CreateUserAsync(UserEntity user);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<UserEntity> GetUserWithAccountsAsync(int id);
        Task UpdateUserAsync(User user);
    }
}