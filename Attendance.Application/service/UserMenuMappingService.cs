using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;

namespace Attendance.Application.Services
{
    public class UserMenuMappingService : IUserMenuMappingService
    {
        private readonly IUserMenuMappingAdaptor _userMenuMappingAdaptor;

        public UserMenuMappingService(IUserMenuMappingAdaptor userMenuMappingAdaptor)
        {
            _userMenuMappingAdaptor = userMenuMappingAdaptor;
        }
        public async Task<IEnumerable<UserMenuMappingDto>> GetAllUserMenuMappings()
        {
            return await _userMenuMappingAdaptor.GetAllUserMenuMappingsAsync();
        }

        public async Task<IEnumerable<UserMenuMappingDto>> GetUserMenuMappingsByUserId(int userId)
        {
            return await _userMenuMappingAdaptor.GetUserMenuMappingsByUserIdAsync(userId);
        }

        public async Task<UserMenuMappingDto> GetUserMenuMappingById(int id)
        {
            return await _userMenuMappingAdaptor.GetUserMenuMappingByIdAsync(id);
        }

        public async Task<string> AddUserMenuMapping(UserMenuMappingDto userMenuMappingDto)
        {
            return await _userMenuMappingAdaptor.AddUserMenuMappingAsync(userMenuMappingDto);
        }

        public async Task<string> UpdateUserMenuMapping(int id, UserMenuMappingDto userMenuMappingDto)
        {
            return await _userMenuMappingAdaptor.UpdateUserMenuMappingAsync(id, userMenuMappingDto);
        }

        public async Task<int> DeleteUserMenuMapping(int id)
        {
            return await _userMenuMappingAdaptor.DeleteUserMenuMappingAsync(id);
        }

        public async Task<string> UpdateUserMenuMappingsForUser(int userId, List<int> menuIds)
        {
            return await _userMenuMappingAdaptor.UpdateUserMenuMappingsForUserAsync(userId, menuIds);
        }
        public async Task<IEnumerable<MenuItemDto>> GetAccessibleMenusByUserId(int userId)
        {
            return await _userMenuMappingAdaptor.GetAccessibleMenusByUserIdAsync(userId);
        }
    }
}
