using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;

namespace Attendance.Application.service
{
    public class MenuMasterService : IMenuMasterService
    {
        private readonly IMenuMasterAdaptor _menuAdaptor;

        public MenuMasterService(IMenuMasterAdaptor menuAdaptor)
        {
            _menuAdaptor = menuAdaptor;
        }

        public async Task<IEnumerable<MenuMasterDto>> GetAllMenuMasters()
        {
            return await _menuAdaptor.GetAllMenuMastersAsync();
        }

        public async Task<IEnumerable<MenuMasterDto>> GetMenuMasterById(int id)
        {
            return await _menuAdaptor.GetMenuMasterByIdAsync(id);
        }
        public async Task<MenuMasterDto> GetById(int id)
        {
            return await _menuAdaptor.GetByIdAsync(id);
        }

        public async Task<string> AddMenuMaster(MenuMasterDto menuMasterDto)
        {
            return await _menuAdaptor.AddMenuMasterAsync(menuMasterDto);
        }

        public async Task<string> UpdateMenuMaster(MenuMasterDto menuMasterDto, int id)
        {
            return await _menuAdaptor.UpdateMenuMasterAsync(menuMasterDto, id);
        }

        public async Task<int> DeleteMenuMaster(int id)
        {
            return await _menuAdaptor.DeleteMenuMasterAsync(id);
        }
    }
}
