using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controllers.Application.DTOs;

namespace Controllers.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task CreateUserAsync(UserDto userDto);
        Task UpdateUserAsync(string userId, UserDto userDto);
        Task DeleteUserAsync(string userId);
    }
}
