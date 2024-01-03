using Bank.WebApi.Interfaces;
using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;
using Bank.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebApi.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserEntity>> Get(int id)
        {
            var userWithAccounts = await _userService.GetUserWithAccountsAsync(id);
            if (userWithAccounts == null) return NotFound();
            return Ok(userWithAccounts);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User userDto)
        {
            var userEntity = new UserEntity
            {
                Name = userDto.Name,
                Address = userDto.Address
            };

            await _userService.CreateUserAsync(userEntity);

            var createdUserDto = new User
            {
                UserId = userEntity.UserId,
                Name = userEntity.Name,
                Address = userEntity.Address
            };

            return CreatedAtAction(nameof(Get), new { id = createdUserDto.UserId }, createdUserDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] UserUpdate user)
        {
            await _userService.UpdateUserAsync(id, user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }

}
