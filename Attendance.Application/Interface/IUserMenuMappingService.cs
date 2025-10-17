using Attendance.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface IUserMenuMappingService
    {
        Task<IEnumerable<UserMenuMappingDto>> GetAllUserMenuMappings();
        Task<IEnumerable<UserMenuMappingDto>> GetUserMenuMappingsByUserId(int userId);
        Task<UserMenuMappingDto> GetUserMenuMappingById(int id);
        Task<string> AddUserMenuMapping(UserMenuMappingDto userMenuMappingDto);
        Task<string> UpdateUserMenuMapping(int id, UserMenuMappingDto userMenuMappingDto);
        Task<int> DeleteUserMenuMapping(int id);
        Task<string> UpdateUserMenuMappingsForUser(int userId, List<int> menuIds);
    }
}
