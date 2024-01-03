using Bank.WebApi.Exceptions;
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
            var userEntity = await _userRepository.GetUserAsync(id) ?? throw new UserNotFoundException("The user does not exist");

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

        public async Task UpdateUserAsync(int id, UserUpdate userDTO)
        {
            _ = await _userRepository.GetUserAsync(id)?? throw new UserNotFoundException("The user does not exist");
            var user = new UserEntity
            {
                UserId = id,
                Name = userDTO.Name,
                Address = userDTO.Address
            };

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            _ = _userRepository.GetUserAsync(id) ?? throw new UserNotFoundException("The user does not exist");
            await _userRepository.DeleteUserAsync(id);
        }
    }
}
