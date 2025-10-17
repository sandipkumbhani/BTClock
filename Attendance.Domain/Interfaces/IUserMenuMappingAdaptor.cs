using Attendance.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface IUserMenuMappingAdaptor
    {
        Task<IEnumerable<UserMenuMappingDto>> GetAllUserMenuMappingsAsync();
        Task<IEnumerable<UserMenuMappingDto>> GetUserMenuMappingsByUserIdAsync(int userId);
        Task<UserMenuMappingDto> GetUserMenuMappingByIdAsync(int id);
        Task<string> AddUserMenuMappingAsync(UserMenuMappingDto userMenuMappingDto);
        Task<string> UpdateUserMenuMappingAsync(int id, UserMenuMappingDto userMenuMappingDto);
        Task<int> DeleteUserMenuMappingAsync(int id);
        Task<string> UpdateUserMenuMappingsForUserAsync(int userId, List<int> menuIds);  // New method
    }
}
