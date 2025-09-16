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

        public async Task<IEnumerable<menuMasterDto>> GetAllMenuMasters()
        {
            return await _menuAdaptor.GetAllMenuMasters();
        }

        public async Task<IEnumerable<menuMasterDto>> GetMenuMasterById(int id)
        {
            return await _menuAdaptor.GetMenuMasterById(id);
        }
        public async Task<menuMasterDto> GetById(int id)
        {
            return await _menuAdaptor.GetById(id);
        }

        public async Task<string> AddMenuMaster(menuMasterDto menu)
        {
            return await _menuAdaptor.AddMenuMaster(menu);
        }

        public async Task<string> UpdateMenuMaster(menuMasterDto menu, int id)
        {
            return await _menuAdaptor.UpdateMenuMaster(menu, id);
        }

        public async Task<int> DeleteMenuMaster(int id)
        {
            return await _menuAdaptor.DeleteMenuMaster(id);
        }
    }
}
