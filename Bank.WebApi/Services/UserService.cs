using Bank.WebApi.Interfaces;
using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;

namespace Bank.WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserEntity> GetUserWithAccountsAsync(int id)
        {
            var userEntity = await _userRepository.GetUserAsync(id);

            if (userEntity == null) return null;

            return new UserEntity
            {
                UserId = userEntity.UserId,
                Name = userEntity.Name,
                Address = userEntity.Address,
                Accounts = userEntity.Accounts
            };
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task CreateUserAsync(UserEntity user)
        {
            await _userRepository.CreateUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }
    }

}
