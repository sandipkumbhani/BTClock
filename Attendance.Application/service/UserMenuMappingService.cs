using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;

namespace Attendance.Application.service
{
    public class UserMenuMappingService : IUserMenuMappingService
    {
        private readonly IUserMenuMappingAdaptor _userMenuMappingAdaptor;

        public UserMenuMappingService(IUserMenuMappingAdaptor userMenuMappingAdaptor)
        {
            _userMenuMappingAdaptor = userMenuMappingAdaptor;
        }

        public async Task<IEnumerable<UserMenuMappingDto>> GetUserMenuById(int id)
        {
            return await _userMenuMappingAdaptor.GetUserMenuById(id);
        }
        public async Task<UserMenuMappingDto> GetById(int id)
        {
            return await _userMenuMappingAdaptor.GetById(id);
        }
        public async Task<IEnumerable<UserMenuMappingDto>> GetAll()
        {
            return await _userMenuMappingAdaptor.GetAll();
        }
        public async Task<string> AddUserMenuMapping(UserMenuMappingDto userMenuMapping)
        {
                return await _userMenuMappingAdaptor.AddUserMenuMapping(userMenuMapping);
        }
        public async Task<string> UpdateMenuMapping(UserMenuMappingDto userMenuMapping, int id)
        {
            return await _userMenuMappingAdaptor.UpdateMenuMapping(userMenuMapping, id);
        }
        public async Task<int> DeleteUserMenuMapping(int id)
        {
            return await _userMenuMappingAdaptor.DeleteUserMenuMapping(id);
        }
        
    }
}
