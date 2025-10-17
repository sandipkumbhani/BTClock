using Attendance.Domain.Models;

namespace Attendance.Application.Interface
{
    public interface IMenuMasterService
    {
        Task<IEnumerable<MenuMasterDto>> GetMenuMasterById(int id);
        Task<MenuMasterDto> GetById(int id);
        Task<IEnumerable<MenuMasterDto>> GetAllMenuMasters();
        Task<string> AddMenuMaster(MenuMasterDto menuMasterDto);
        Task<string> UpdateMenuMaster(MenuMasterDto menuMasterDto, int id);
        Task<int> DeleteMenuMaster(int id);
    }
}
