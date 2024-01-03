using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;

namespace Bank.WebApi.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(UserEntity user);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<UserEntity> GetUserAsync(int id);
        Task UpdateUserAsync(UserEntity user);
        public Task<bool> UserExists(int userId);
    }
}