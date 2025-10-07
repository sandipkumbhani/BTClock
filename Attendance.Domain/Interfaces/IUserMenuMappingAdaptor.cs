using Attendance.Domain.Models;

namespace Attendance.Domain.Interfaces
{
    public interface IUserMenuMappingAdaptor
    {
        Task<IEnumerable<UserMenuMappingDto>> GetUserMenuById(int id);
        Task<UserMenuMappingDto> GetById(int id);
        Task<IEnumerable<UserMenuMappingDto>> GetAll();
        Task<string> AddUserMenuMapping(UserMenuMappingDto userMenuMapping);
        Task<string> UpdateMenuMapping(UserMenuMappingDto userMenuMapping, int id);

        Task<int> DeleteUserMenuMapping(int id);
        Task<List<UserDto>> GetAllUserAsync();
    }
}
